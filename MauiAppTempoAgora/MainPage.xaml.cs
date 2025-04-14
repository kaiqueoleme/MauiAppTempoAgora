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

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
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
                                         $"Visibilidade: {t.visibility}km \n";

                        lbl_res.Text = dados_previsao;

                        string mapa = $"https://embed.windy.com/embed.html?" +
                            $"type=map&location=coordinates&metricRain=mm&metricTemp=°C" +
                            $"&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                            $"&lat={t.lat}&lon={t.lon}";

                        wv_mapa.Source = mapa;
                    } else
                    {
                        lbl_res.Text = "Sem dados de Previsão.";
                    }
                } else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Sem Conexão", "Não foi possível estabelecer uma conexão com a internet.", "OK");
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

        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(
                    GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)
                    );

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude}";
                    lbl_coords.Text = local_disp;

                    // pega nome da cidade que está nas coordenadas.
                    GetCidade(local.Latitude, local.Longitude);
                }

            } 
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Erro: Dispositivo não Suporta", ex.Message, "Ok");
            }
            catch (FeatureNotEnabledException ex)
            {
                await DisplayAlert("Erro: Localização Desabilitada", ex.Message, "Ok");
            }
            catch (PermissionException ex)
            {
                await DisplayAlert("Erro: Permissão da Localização", ex.Message, "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
            }
        }

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                if (place != null)
                {
                    txt_cidade.Text = place.Locality;
                } 
            } 
            catch (Exception ex)
            {
                await DisplayAlert("Erro: Obtenção do nome da Cidade", ex.Message, "Ok");
            }
        }
    }

}
