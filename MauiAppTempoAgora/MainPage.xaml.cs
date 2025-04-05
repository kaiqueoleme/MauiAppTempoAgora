using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);
                   
                    if (t != null)
                    {
                        string dados_previsao = "";
                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Pôr do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n" +
                                         $"Clima: {t.description} \n" +
                                         $"Velocidade do Vento: {t.speed}m/s \n" +
                                         $"Visibilidade: {t.visibility} metros vísiveis \n";

                        lbl_res.Text = dados_previsao;
                    } else
                    {
                        lbl_res.Text = "Sem dados de Previsão.";
                    }
                } else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            } 
            catch (Exception ex)
            {
                if (ex.Message == "NotFound")
                {
                    lbl_res.Text = "Cidade não encontrada";
                } else
                {
                    await DisplayAlert("Ops", ex.Message, "Ok");
                }
            }
        }
    }

}
