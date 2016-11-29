using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace TestBitmap
{
    public class Image
    {
        private Bitmap _image;
        private IntPtr _buffer = IntPtr.Zero;
        private BitmapData _bitmapData = null;
        private int _pixelCount;

        public System.Drawing.Image TheImage { get { return _image; } }
        public int Width { get { return _image.Width; } }
        public int Height { get { return _image.Height; } }
        public ColorPalette Palette
        {
            get { return _image.Palette; }
            set { _image.Palette = value; }
        }

        public Image(int width, int height)
        {
            _image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            _pixelCount = width * height;
        }

        public class EditContext
        {
            private Image _image;

            private EditContext() { }
            public EditContext(Image image)
            {
                _image = image;
            }

            public byte this[int x, int y]
            {
                get
                {
                    return GetPixel(x, y);
                }
                set
                {
                    SetPixel(x, y, value);
                }
            }

            /// <summary>
            /// Get the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public byte GetPixel(int x, int y)
            {
                // Get start index of the specified pixel
                int i = ((y * _image.Width) + x);

                if (i > _image._pixelCount - 1)
                    throw new IndexOutOfRangeException();

                // For 8 bpp get color value (Red, Green and Blue values are the same)
                unsafe
                {
                    byte* b = (byte*)_image._buffer.ToPointer();
                    return b[i];
                }
            }

            /// <summary>
            /// Set the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="color"></param>
            public void SetPixel(int x, int y, byte index)
            {
                // Get start index of the specified pixel
                int i = ((y * _image.Width) + x);

                // For 8 bpp set color value (Red, Green and Blue values are the same)
                unsafe
                {
                    byte* b = (byte*)_image._buffer.ToPointer();
                    b[i] = index;
                }
            }
        }

        public void Edit(Action<EditContext> operation)
        {
            lockBits();
            operation(new EditContext(this));
            unlockBits();
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        private void lockBits()
        {
            try
            {
                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // Lock bitmap and return bitmap data
                _bitmapData = _image.LockBits(rect, ImageLockMode.ReadWrite,
                                             _image.PixelFormat);

                // create byte array to copy pixel values
                _buffer = _bitmapData.Scan0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        private void unlockBits()
        {
            try
            {
                // Unlock bitmap data
                _image.UnlockBits(_bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
