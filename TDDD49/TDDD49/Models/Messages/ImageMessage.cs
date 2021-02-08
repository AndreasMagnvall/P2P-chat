using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
namespace TDDD49.Models
{
    public class ImageMessage : Message
    {
        public string ImagePath { get; set; }

        public BitmapImage Bitmap
        {
            get
            {
                Uri resourceUri = new Uri(ImagePath, UriKind.Absolute);
                BitmapImage bmap =  new BitmapImage(resourceUri);
                return bmap; 
            }

        }

        public ImageMessage(string text, string message, string sender, string date) : base(message, sender, date)
        {
            this.ImagePath = text;
        }

        public ImageMessage(string text, string sender) : base("Ok", sender, "1996")
        {
            this.ImagePath = text;
        }

        public ImageMessage() : base("Ok", "Jag", "1996")
        {

        }
    }
}
