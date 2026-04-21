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
- **Scrutor** → Decorator Pattern ile dependency injection desteği  
- **Docker & Docker Compose** → Konteyner yönetimi  
- **Entity Framework Core** → ORM ve veri erişim yönetimi  
- **SQL Server** → Ordering servisi için ilişkisel veritabanı  

---

## 🧩 Microservices

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

---

## 🔗 Inter-Service Communication (gRPC & REST)

Servisler arası iletişim için farklı yaklaşımlar kullanılmıştır:

- **gRPC** → Basket ve Discount servisleri arasında yüksek performanslı senkron iletişim  
- **REST API** → Client ve API Gateway üzerinden erişim  

---


## 🧠 Ordering Microservice (Advanced DDD)

Ordering mikroservisi, karmaşık iş kurallarını yönetmek amacıyla **Domain-Driven Design (DDD)** prensipleri ile geliştirilmiştir.

### ⚙️ Kullanılan DDD Yaklaşımları

- **Tactical DDD Patterns**  
  Entity, Value Object ve Aggregate Root yapıları kullanılarak zengin bir domain modeli oluşturulmuştur.

- **Strongly Typed IDs**  
  Primitive Obsession’ı önlemek için tüm kimlikler (Id) ve değer nesneleri özel tiplerle modellenmiştir.

- **Encapsulation**  
  İş kuralları tamamen domain katmanı içinde kapsüllenmiş, tutarsız veri oluşumu nesne seviyesinde engellenmiştir.

- **Domain Events**  
  Sistem içi yan etkiler domain event’ler ile yönetilerek gevşek bağlı (loosely coupled) bir yapı sağlanmıştır.

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

- **Logging**
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

1. **Okuma (Cache-Aside):**  
   Önce Redis kontrol edilir. Veri varsa hızlıca döner, yoksa veritabanından çekilip cache’e yazılır.

2. **Yazma / Güncelleme:**  
   Veri hem veritabanına hem Redis’e yazılır.

3. **Silme:**  
   Hem cache hem veritabanı temizlenir.

---

## 🏥 Health Checks

- `http://localhost:6000/health` → Servis ve bağımlılıkların durumu  

---

## 🛠️ Kurulum ve Çalıştırma

1. **Docker Desktop** çalışır durumda olmalıdır  

2. Terminalde proje dizinine gidin:

```bash
docker-compose up -d
````

3. API projelerini çalıştırın

4. API’leri test edin:

### 🔹 Swagger UI

* Catalog API → http://localhost:6000/swagger
* Basket API → http://localhost:6001/swagger

### 🔹 Postman

* Endpoint’ler Postman üzerinden de test edilebilir

> Not: Portlar proje ayarlarına göre değişebilir.

