using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTSC.PubSub
{
    public class ImageProcessedPayload
    {
        public ImageProcessedPayload(Bitmap image)
        {
            Image = image;
        }

        public Bitmap Image { get; }
    }
    public class ImageProcessedEvent : Prism.Events.PubSubEvent<ImageProcessedPayload>
    {

    }
}
