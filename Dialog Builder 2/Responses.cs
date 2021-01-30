using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog_Builder_2
{
    class Responses
    {
        private string responsesText = "";
        private string responsesCondition = "";

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
    }
}
