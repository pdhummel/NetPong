using System;
using System.Windows.Forms;

namespace Pong;

public partial class Form1 : Form
{
    private PongGame? gameObject;
    private Server? server;

    private Client? client;

    public Form1()
    {
        FormClosed += new System.Windows.Forms.FormClosedEventHandler(Form1_FormClosed);
        InitializeComponent();
        Text = "Pong";
        Size = new Size(200, 300);
        MaximizeBox = false;
        WindowState = FormWindowState.Normal;
        buttonHost.Click += new System.EventHandler(ButtonHost_Click);
        buttonLeft.Click += new System.EventHandler(ButtonLeft_Click);
        buttonRight.Click += new System.EventHandler(ButtonRight_Click);
    }


    public IntPtr GetDrawSurface()
    {
        //return pictureBox1.Handle;
        return this.Handle;
    }

    public void SetGameObject(PongGame game)
    {
        gameObject = game;
    }

    public void SetServer(Server server)
    {
        this.server = server;
    }

    public void SetClient(Client client)
    {
        this.client = client;
    }

    private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
    {
        Application.Exit();
    }

    private void ButtonHost_Click(object? sender, EventArgs e)
    {
        Console.WriteLine("ButtonHost_Click(): Host Button Clicked");
        server?.StartAsHost(5005, 2, "pong");
        buttonHost.Enabled = false;
    }

    private void ButtonLeft_Click(object? sender, EventArgs e)
    {
        Console.WriteLine("ButtonLeft_Click(): Left Button Clicked");
        client?.Connect("127.0.0.1", 5005, "pong", "left");
        buttonLeft.Enabled = false;
        buttonRight.Enabled = false;
        //pictureBox1.Focus();
        this.Hide();
        gameObject?.CreateLeftPaddle();
        gameObject?.CreateRightPaddle();
    }

    private void ButtonRight_Click(object? sender, EventArgs e)
    {
        Console.WriteLine("ButtonRight_Click(): Right Button Clicked");
        client?.Connect("127.0.0.1", 5005, "pong", "right");
        buttonLeft.Enabled = false;
        buttonRight.Enabled = false;
        //pictureBox1.Focus();
        this.Hide();
        gameObject?.CreateRightPaddle();
        gameObject?.CreateLeftPaddle();
    }    

}
