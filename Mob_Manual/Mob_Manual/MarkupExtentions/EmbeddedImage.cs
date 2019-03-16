using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mob_Manual.MarkupExtentions
{
    [ContentProperty("ResourceId")]
    public class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(ResourceId))
            {
                return null;
            }
            return ImageSource.FromResource("Mob_Manual.Images.background.jpg");
        }
    }
}
