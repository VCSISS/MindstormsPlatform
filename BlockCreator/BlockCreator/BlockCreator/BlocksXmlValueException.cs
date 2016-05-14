using System;
using System.Collections.Generic;
using System.Text;

namespace BlockCreatorLogic.Exceptions
{
    /// <summary>
    /// A class representing an exception that occurs when an value
    /// that is used in generating the blocks.xml file is invalid;
    /// such as a module name, block family, etc. This is probably
    /// the most common type of exception in the BlockCreator source code,
    /// and all controllers (and possibly views) using this model as
    /// part of MVC will have to handle it for any method calls to
    /// <see cref="BlockCreatorActions"/>.
    /// </summary>
    public class BlocksXmlValueException : ArgumentException
    {
        private string _valueName;
        public string ValueName
        {
            get
            {
                return _valueName;
            }

            set
            {
                _valueName = value;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
            }
        }

        public BlocksXmlValueException(string value, string error) :
            base(error, value)
        {
            _valueName = value;
            _errorMessage = error;
        }
    }
}
