using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busqueda3Niveles
{
    public partial class Form1 : Form
    {
        string texto;
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Resultados.Items.Clear();
            cargaListBox();
        }

        private void cargaListBox()
        {
            texto = cuadroBusqueda.Text;
            if(cuadroBusqueda.Text.Contains(" "))
            {
                texto = cuadroBusqueda.Text.Replace(" ","-");
            }
            WebRequest request = WebRequest.Create("https://genius.com/artists/" + texto + "/albums");
            Thread.Sleep(3000);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            string html = responseFromServer;
            Regex r = new Regex("<h3 class=\"ListItem__Title-sc-122yj9e-4 nknYf\">(.*?)</h3>");
            MatchCollection matches = r.Matches(html);

            if (matches.Count > 0 )
            {
                foreach(Match match in matches)
                {
                    string artista = match.Groups[1].Value;
                    Resultados.Items.Add(artista);
                }
            }
        }

        private void Resultados_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if(Resultados.SelectedIndex != -1)
            {
                string album = Resultados.SelectedItem.ToString();
                cargaTextBox(album);
            }
        }
        
        private void cargaTextBox(string album)
        {
            string nuevoAlbum = album;
            if(album.Contains(" "))
            {
                nuevoAlbum = album.Replace(" ", "-");
            }
            WebRequest request = WebRequest.Create("https://genius.com/albums/" + texto + "/"+ nuevoAlbum);
            Thread.Sleep(3000);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            string html = responseFromServer;
            Regex r = new Regex("<h3 class=\"chart_row-content-title\">\\s*(.*?)\\s*<span");
            MatchCollection matches = r.Matches(html);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string cancion = match.Groups[1].Value;
                    richTextBox1.AppendText(cancion);
                    richTextBox1.AppendText("\n");
                }
            }
        }
    }
}
