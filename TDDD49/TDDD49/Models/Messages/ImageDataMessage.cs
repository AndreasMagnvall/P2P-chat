using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models.Messages
{
    public class ImageDataMessage : Message
    {
        public string base64Image;
        public string extension; // TODO: GÖR OM TILL ENUM
        public ImageDataMessage(string base64str, string extension, string sender) : base("THEEN", sender, "10")
        {
            this.base64Image = base64str;
            this.extension = extension;
        }

        public byte[] ImageBytes() {
            return Convert.FromBase64String(base64Image);
        }
    }
}
