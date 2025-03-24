using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null) 
                    {
                        string dados_previsao = "";
                        dados_previsao += $"Latitude: {t.lat}m/s\n";
                        dados_previsao += $"Longitude: {t.lon}m/s\n";
                        dados_previsao += $"Cidade: {txt_cidade.Text}\n";
                        dados_previsao += $"Temperatura Mínima: {t.temp_min}°C\n";
                        dados_previsao += $"Temperatura Máxima: {t.temp_max}°C\n";
                        dados_previsao += $"Velocidade do Vento: {t.speed}m/s\n";
                        dados_previsao += $"Visibilidade: {t.visibility}m\n";
                        dados_previsao += $"Nascer do Sol: {t.sunrise}\n";
                        dados_previsao += $"Pôr do Sol: {t.sunset}\n";

                        lbl_res.Text = dados_previsao;

                    }
                    else
                    {
                        lbl_res.Text = "Cidade não encontrada";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade";
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}
