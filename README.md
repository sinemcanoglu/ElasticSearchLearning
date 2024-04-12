# ElasticSearchLearning

## Proje Açıklaması
Bu proje, ElasticSearch üzerinde çeşitli arama ve analiz işlemleri gerçekleştiren bir API sunar. Web günlükleri üzerinde detaylı sorgulamalar yaparak, IP adresi bazında aramalar, tarih aralıklarına göre filtrelemeler, yanıt kodlarına göre sınıflandırmalar ve alanlara göre istatistiksel veri toplama gibi işlevleri destekler. Bu API, web günlüklerinden anlamlı bilgiler çıkarmak için güçlü bir araçtır.

Projede kullanılan veriler, Kibana'nın sağladığı `kibana_sample_data_logs` datasetinden alınmıştır, bu dataset geniş çeşitlilikteki log kayıtlarını içerir ve gerçek dünya senaryolarını simüle eder.

## Özellikler
- **Temel Arama**: Basit sorgu metinlerini kullanarak günlüklerde arama yapma.
- **Mesajlara Göre Arama**: Belirli mesaj içeriklerine göre günlükleri filtreleme.
- **IP Adresine Göre Arama**: Spesifik IP adreslerine göre günlük sorgulama.
- **Tarih Aralığına Göre Arama**: Belirlenen tarih aralıklarında günlükleri sorgulama.
- **Yanıt Kodlarına Göre Arama**: HTTP yanıt kodlarına göre günlükleri filtreleme.
- **Alan İstatistikleri**: Belirli bir alanla ilgili istatistiksel bilgiler toplama.
