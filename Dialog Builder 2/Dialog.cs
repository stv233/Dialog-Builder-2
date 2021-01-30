using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog_Builder_2
{
    class Dialog
    {
        private string localVariable = "";

        private string globalVariable = "";

        private List<Page> pages = new List<Page>();

        public string LocalVariable
        {
            get
            {
                return localVariable;
            }
            set
            {
                localVariable = value;
            }
        }


        public string GlobalVariable
        {
            get
            {
                return globalVariable;
            }
            set
            {
                globalVariable = value;
            }
        }

    }
}
