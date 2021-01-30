using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog_Builder_2
{
    class Response
    {
        private string responsesText = "";
        private string responsesCondition = "";
        private string responsesLink = "0";

        public string Text
        {
            get
            {
                return responsesText;
            }
            set
            {
                responsesText = value;
            }
        }

        public string Condition
        {
            get
            {
                return responsesCondition;
            }
            set
            {
                responsesCondition = value;
            }
        }

        public string Link
        {
            get
            {
                return responsesLink;
            }
            set
            {
                responsesLink = value;
            }
        }

        public Response() { }

        public Response(string text)
        {
            Text = text;
        }
    }
}
