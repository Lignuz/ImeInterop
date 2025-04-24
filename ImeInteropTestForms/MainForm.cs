using ImeInterop;

namespace ImeInteropTestForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            ImeManager imeManager = new ImeManager();
            imeManager.Attach(textBox1, 
                onComposition: (s, e) => label1.Text = "Composing: " + e.CompositionText,
                onStart: (s, _) => label1.Text = "IME started", 
                onEnd: (s, _) => label1.Text = "IME ended");
        }
    }
}