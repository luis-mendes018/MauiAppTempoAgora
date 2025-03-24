using System;
using System.Collections.Generic;

using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked_Previsao(object sender, EventArgs e)
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

                    string mapa = 
                   $"https://embed.windy.com/embed.html?type=map&location=coordinates&metricRain=mm&metricTemp=°C&metricWind=km/h&zoom=3&overlay=wind&product=ecmwf&level=surface&lat={t.lat?.ToString().Replace(",", ".")}&lon={t.lon?.ToString().Replace(",", ".")}";

                   wv_mapa.Source = mapa;

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

    private async void Button_Clicked_Localizacao(object sender, EventArgs e)
    {
        try
        {
            GeolocationRequest request = 
                new GeolocationRequest
                (GeolocationAccuracy.Medium, 
                 TimeSpan.FromSeconds(10)
                );

            Location? local =  await Geolocation.Default.GetLocationAsync(request);

            if (local != null)
            {
                string local_disp = $"Latitude: {local.Latitude}\n" +
                                    $"Longitude: {local.Longitude}\n";

                lbl_coords.Text = local_disp;

                //Obtém o nome da cidade onde está as coordenadas
                GetCidade(local.Latitude, local.Longitude);
            }
            else
            {
                lbl_res.Text = "Localização não encontrada";
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {

           await DisplayAlert("Erro: O dispositivo não Suporta esse recurso", fnsEx.Message, "OK");
        }
        catch(FeatureNotEnabledException fnSex)
        {
           await DisplayAlert("Erro: Localização não habilitada", fnSex.Message, "OK");
        }
        catch (PermissionException pEx)
        {
           await DisplayAlert("Erro: Permissão negada", pEx.Message, "OK");
        }
        catch (Exception ex)
        {
           await DisplayAlert("Erro", ex.Message, "OK");
        }   

    }

    private async void GetCidade(double lat, double lon)
    {
        try
        {
            IEnumerable<Placemark> places =
            await Geocoding.Default.GetPlacemarksAsync(lat, lon);

            Placemark? place = places.FirstOrDefault();

            if (place != null)
            {
                txt_cidade.Text = place.Locality;
            }
        }
        catch (Exception ex)
        {

            await DisplayAlert("Erro: ", ex.Message, "OK");
        }
        
    }
}
