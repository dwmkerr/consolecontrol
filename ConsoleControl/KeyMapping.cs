using System.Windows.Forms;

namespace ConsoleControl
{
    /// <summary>
    /// A KeyMapping defines how a key combination should
    /// be mapped to a SendKeys message.
    /// </summary>
    public class KeyMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        public KeyMapping()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        /// <param name="control">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="keyCode">The key code.</param>
        /// <param name="sendKeysMapping">The send keys mapping.</param>
        /// <param name="streamMapping">The stream mapping.</param>
        public KeyMapping(bool control, bool alt, bool shift, Keys keyCode, string sendKeysMapping, string streamMapping)
        {
            //  Set the member variables.
            IsControlPressed = control;
            IsAltPressed = alt;
            IsShiftPressed = shift;
            KeyCode = keyCode;
            SendKeysMapping = sendKeysMapping;
            StreamMapping = streamMapping;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is control pressed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is control pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsControlPressed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether alt is pressed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is alt pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsAltPressed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is shift pressed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is shift pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsShiftPressed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the key code.
        /// </summary>
        /// <value>
        /// The key code.
        /// </value>
        public Keys KeyCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the send keys mapping.
        /// </summary>
        /// <value>
        /// The send keys mapping.
        /// </value>
        public string SendKeysMapping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stream mapping.
        /// </summary>
        /// <value>
        /// The stream mapping.
        /// </value>
        public string StreamMapping
        {
            get;
            set;
        }
    }
}