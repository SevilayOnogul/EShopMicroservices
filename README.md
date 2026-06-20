# 🛒 EShop Microservices

Bu proje, mikroservis mimarisi kullanılarak geliştirilen bir e-ticaret sistemidir.  
Sistem, farklı iş alanlarına ayrılmış bağımsız servislerden oluşur ve modern yazılım mimarileri ile geliştirilmiştir.

---

## 🚀 Kullanılan Teknolojiler ve Kütüphaneler

- **.NET 8 / ASP.NET Core**
- **Marten** → PostgreSQL üzerinde Document DB yetenekleri sağlar  
- **MediatR** → CQRS deseni için  
- **Carter** → Minimal API endpoint’leri  
- **Mapster** → Nesne eşleme (mapping)  
- **Redis** → Dağıtık cache yönetimi (Basket Service)  
- **gRPC** → Servisler arası yüksek performanslı iletişim
- **RabbitMQ** → Asynchronous message broker
- **MassTransit** → RabbitMQ tabanlı message bus yönetimi
- **Scrutor** → Decorator Pattern ile dependency injection desteği  
- **Docker & Docker Compose** → Konteyner yönetimi  
- **Entity Framework Core** → ORM ve veri erişim yönetimi  
- **SQL Server** → Ordering servisi için ilişkisel veritabanı
- **YARP API Gateway** → Tersine proxy ve merkezi istek yönlendirme
- **Refit** → Tip güvenli (Type-Safe) REST istemci yönetimi
- **Razor Pages** → UI ve web arayüz yönetimi

---

## 🧩 Microservices

### 🛡️ YARP API Gateway
Tüm istemci (client) isteklerini tek bir noktadan karşılayan ve ilgili mikroservislere güvenli bir şekilde yönlendiren tersine proxy (Reverse Proxy) katmanıdır.

- **YARP (Yet Another Reverse Proxy)** kütüphanesi kullanılmıştır.
- Rotalama (Routing) yapılandırmaları dinamik olarak yönetilir.
- Tüm mikroservislere dış dünyadan tek bir ortak port üzerinden erişim sağlar.
- Gateway URL → `https://localhost:6064` (HTTPS) / `http://localhost:6004` (HTTP)

---

### 🌐 Web Application (Shopping.Web)
Kullanıcıların ürünleri listelediği, sepet işlemlerini yönettiği ve sipariş verdiği frontend katmanıdır.

- **ASP.NET Core Razor Pages** mimarisi ile geliştirilmiştir.
- **Refit** kütüphanesi kullanılarak mikroservis API'leri ile tip güvenli (Type-Safe) ve deklaratif bir şekilde haberleşir.
- `ICatalogService`, `IBasketService` ve `IOrderingService` soyutlamaları üzerinden Gateway ile konuşur.
- Sayfalama (Pagination) yanıtlarını backend ile tam uyumlu karşılayacak `PaginatedResult` yapısını içerir.

---

### 📦 Catalog API
Ürün yönetimi işlemlerini sağlar.

- Ürün ekleme, güncelleme, silme  
- Kategoriye göre filtreleme  
- PostgreSQL + Marten kullanımı  

---

### 🛒 Basket API
Kullanıcı sepet işlemlerini yönetir.

- Sepete ürün ekleme / silme  
- Sepet görüntüleme  
- Redis ile yüksek performanslı cache yönetimi  
- Cache-Aside Pattern implementasyonu  
- Discount servisi ile entegre çalışarak indirimli fiyat hesaplama  

---

### 💸 Discount gRPC Service
İndirim hesaplamalarını yöneten yüksek performanslı bir mikroservistir.

- gRPC protokolü ile servisler arası senkron iletişim sağlar  
- Ürünlere uygulanan indirimleri hesaplar  
- Basket servisi tarafından gRPC Client ile tüketilir  
- SQLite veritabanı kullanılarak indirim verileri yönetilir  
- gRPC Endpoint → `grpc://localhost:6062` (Docker) / `grpc://localhost:5052` (Local)

---

### 🧾 Ordering API
Sipariş yönetimini Domain-Driven Design prensipleriyle ele alan mikroservistir.

- DDD (Domain-Driven Design) yaklaşımı ile geliştirilmiştir  
- Entity, Value Object ve Aggregate Root yapıları içerir  
- CQRS ve Clean Architecture ile yapılandırılmıştır  
- EF Core Code First yaklaşımı ile geliştirilmiştir  
- SQL Server veritabanı kullanılmaktadır  

---

## 🏗️ Mimari Yapı

Projede aşağıdaki modern mimari yaklaşımlar uygulanmıştır:

- **Microservices Architecture**
- **CQRS (Command Query Responsibility Segregation)**
- **Vertical Slice Architecture**
- **Decorator Pattern**
- **Cache-Aside Pattern**
- **gRPC Communication (Inter-service)**
- **Event-Driven Architecture**
- **Asynchronous Messaging with RabbitMQ**
- **API Gateway Pattern (YARP)**
- **Type-Safe REST Communication (Refit)**

---

## 🔗 Inter-Service Communication (gRPC, REST & Messaging)

Servisler arası iletişim için farklı yaklaşımlar kullanılmıştır:

- **gRPC** → Basket ve Discount servisleri arasında yüksek performanslı senkron iletişim  
- **REST API** → Client ve API Gateway üzerinden erişim
- **RabbitMQ + MassTransit** → Basket ve Ordering servisleri arasında asynchronous event-driven communication sağlar
  

---

## 📨 Async Order Processing with RabbitMQ

Checkout işlemi sırasında servisler arası iletişim asynchronous messaging yapısı ile sağlanmaktadır.

### 🔄 İşleyiş Akışı

1. Kullanıcı checkout işlemini başlatır  
2. Basket servisi `BasketCheckoutEvent` event’ini publish eder  
3. RabbitMQ event’i queue üzerinden iletir  
4. Ordering servisi event’i consume ederek sipariş oluşturma sürecini başlatır  
5. Başarılı işlem sonrası kullanıcının sepeti temizlenir  

### ⚙️ Kullanılan Teknolojiler

- **RabbitMQ** → Message Broker
- **MassTransit** → Messaging abstraction ve consumer yönetimi
- **Event-Driven Architecture** → Loose coupling ve scalable communication

---

## 🧠 Ordering Microservice (Advanced DDD)

Ordering mikroservisi, karmaşık iş kurallarını yönetmek amacıyla **Domain-Driven Design (DDD)** prensipleri ile geliştirilmiştir.

### ⚙️ Kullanılan DDD Yaklaşımları

- **Tactical DDD Patterns** Entity, Value Object ve Aggregate Root yapıları kullanılarak zengin bir domain modeli oluşturulmuştur.
- **Strongly Typed IDs** Primitive Obsession’ı önlemek için tüm kimlikler (Id) ve değer nesneleri özel tiplerle modellenmiştir.
- **Encapsulation** İş kuralları tamamen domain katmanı içinde kapsüllenmiş, tutarsız veri oluşumu nesne seviyesinde engellenmiştir.
- **Domain Events** Sistem içi yan etkiler domain event’ler ile yönetilerek gevşek bağlı (loosely coupled) bir yapı sağlanmıştır.

---

## ⚙️ Ordering Application Layer (CQRS & MediatR)

Ordering mikroservisinin kalbi olan bu katman, **Clean Architecture** ve **CQRS** prensiplerine göre yapılandırılmıştır.

### 🛠️ Kullanılan Teknolojiler ve Desenler
- **MediatR:** Command ve Query süreçlerini birbirinden ayırarak gevşek bağlı (loosely coupled) bir mimari sağlar.
- **FluentValidation:** İsteklerin iş mantığına girmeden önce doğrulanmasını garanti altına alır.
- **Mapster & Manual Mapping:** Domain modelleri ile DTO'lar arasındaki dönüşümler, performans odaklı extension metodları ile yönetilir.

### ✨ Temel Özellikler
- **CQRS Yapısı:**
  - **Commands:** Sipariş oluşturma, güncelleme ve silme işlemleri (Create, Update, Delete).
  - **Queries:** İsme göre, müşteriye göre filtreleme ve gelişmiş sayfalama (Pagination) destekli listeleme işlemleri.
- **Domain Events:** Sipariş süreçlerindeki değişimleri (Örn: `OrderCreated`, `OrderUpdated`) takip eden ve sistem içi iletişimi sağlayan asenkron **EventHandler** yapıları.
- **Sayfalama (Pagination):** `BuildingBlocks` katmanında tanımlanan global yapı sayesinde büyük veri setleri performanslı bir şekilde sunulur.
- **Hata Yönetimi:** Uygulamaya özel `OrderNotFoundException` gibi hata sınıfları ile anlamlı geri bildirimler sağlanır.

---

## 🌐 Ordering API Layer (Minimal API & Carter)

Ordering mikroservisinin dış dünyaya açılan katmanıdır.  
Minimal API yaklaşımı ile sade, performanslı ve okunabilir endpoint’ler oluşturulmuştur.

### ⚙️ Kullanılan Yapılar
- **Carter:** Minimal API endpoint’lerini modüler hale getirir  
- **REPR Pattern (Request-Endpoint-Response):** Endpoint organizasyonunu sadeleştirir  
- **Minimal API:** Controller yapısına alternatif, daha lightweight API tasarımı  

### ✨ Özellikler
- Endpoint’ler feature bazlı olarak organize edilmiştir  
- Request ve Response modelleri açık ve ayrıştırılmıştır  
- CQRS yapısına uygun olarak Command ve Query endpoint’leri ayrılmıştır
  
---

## 💰 Discount Integration Flow

Sepete ürün eklenirken indirim hesaplama süreci aşağıdaki şekilde çalışır:

1. Basket servisi, eklenen ürün için Discount servisine gRPC isteği gönderir  
2. Discount servisi ilgili ürüne ait indirim bilgisini döner  
3. Gelen indirim tutarı ürün fiyatına uygulanır  
4. Güncellenmiş (indirimli) fiyat sepet içine kaydedilir  

Bu yapı sayesinde **gerçek zamanlı fiyat hesaplama** ve **servisler arası senkron veri akışı** sağlanır.

---

## 📁 Klasör Yapısı (Vertical Slice)

Bu projede katmanlar yerine **feature bazlı yapı** kullanılmıştır.  

Her feature klasörü (`CreateProduct`, `GetBasket` vb.) şu bileşenleri içerir:

- **Command / Query** → İş mantığı  
- **Handler** → İşlem yönetimi  
- **Validator** → FluentValidation  
- **Endpoint** → API rotası  

---

## 🧩 Cross-Cutting Concerns

### 📊 Merkezi Loglama (Centralized Logging) & İzlenebilirlik

Projedeki mikroservislerin log yönetimi, **Serilog** kütüphanesi ve **Seq** paneli entegrasyonu ile merkezi bir yapıya taşınmıştır. Kod tekrarını önlemek amacıyla loglama konfigürasyonları `BuildingBlocks` katmanı üzerinde genişletme metodu (Extension Method) olarak kurgulanmıştır.

#### Kullanılan Teknolojiler & Paketler
* **Serilog.AspNetCore:** Temel loglama mimarisi.
* **Serilog.Sinks.Seq:** Logların merkezi Seq sunucusuna asenkron fırlatılması.
* **Serilog.Enrichers:** Log verilerine `ApplicationName`, `MachineName` ve `ProcessId` gibi meta verilerin otomatik eklenmesi.

- **Global Exception Handling**
- **Validation Pipeline**

---

## 📡 API Endpoints

### Catalog API
- `GET /products`
- `GET /products/{id}`
- `GET /products/category/{category}`
- `POST /products`
- `PUT /products`
- `DELETE /products/{id}`

### Basket API
- `GET /basket/{userName}`
- `POST /basket`
- `DELETE /basket/{userName}`

---

## ⚡ Caching Strategy (Redis)

Basket mikroservisinde performans optimizasyonu için **Distributed Caching** uygulanmıştır:

1. **Okuma (Cache-Aside):** Önce Redis kontrol edilir. Veri varsa hızlıca döner, yoksa veritabanından çekilip cache’e yazılır.
2. **Yazma / Güncelleme:** Veri hem veritabanına hem Redis’e yazılır.
3. **Silme:** Hem cache hem veritabanı temizlenir.

---

## 🏥 Health Checks

- `https://localhost:6064/health` → Gateway portu üzerinden tüm servis ve bağımlılıkların sağlık durumu tek noktadan izlenebilir.

---

## 🛠️ Kurulum ve Çalıştırma

1. **Docker Desktop** çalışır durumda olmalıdır  

2. Terminalde proje dizinine gidin ve sistemi ayağa kaldırın:
   
```bash
docker-compose up -d
```

🔹 API Gateway & Service URLs
Artık mikroservislere doğrudan bağlanmak yerine merkezi YARP Gateway (6064) üzerinden erişim sağlanmaktadır:

- API Gateway Root (HTTPS): https://localhost:6064

- Catalog API Endpoints: https://localhost:6064/catalog-service/products

- Basket API Endpoints: https://localhost:6064/basket-service/basket/{userName}

- Web UI (Shopping.Web): https://localhost:6065 veya http://localhost:6005

🔹 Swagger UI
Mikroservislerin API dokümantasyonlarına da doğrudan YARP Gateway üzerinden güvenli bir şekilde erişebilirsiniz:

- Catalog API Swagger: https://localhost:6064/catalog-service/swagger

- Basket API Swagger: https://localhost:6064/basket-service/swagger

🔹 Message Broker UI
- RabbitMQ Management: http://localhost:15672 (User: guest, Pass: guest)

🔹 Postman
- Tüm endpoint'ler Postman üzerinden https://localhost:6064 ana adresi (Gateway) kullanılarak test edilebilir.

Not: Postman üzerinden HTTPS istekleri atarken yerel sertifika hatalarıyla karşılaşmamak için Postman ayarlarından SSL certificate verification seçeneğini kapatmanız önerilir.

