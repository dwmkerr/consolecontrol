using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsoleControlSample
{
  /// <summary>
  /// The new process form.
  /// </summary>
  public partial class FormNewProcess : Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="FormNewProcess"/> class.
    /// </summary>
    public FormNewProcess()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    /// <value>
    /// The name of the file.
    /// </value>
    public string FileName
    {
      get { return textBoxFileName.Text; }
    }

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    public string Arguments
    {
      get { return textBoxArguments.Text; }
    }
  }
}
