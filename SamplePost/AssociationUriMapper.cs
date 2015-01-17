using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SamplePost
{
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;
        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            // URI association launch for my app detected
            if (tempUri.Contains("vasoo:"))
            {
                return new Uri("/MainPage.xaml?bilgi=" +uri.ToString(), UriKind.Relative);
            }
            // Otherwise perform normal launch.
            return uri;
        }
    }
}
