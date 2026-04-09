# 🛒 EShop Microservices - Catalog API

Bu proje, mikroservis mimarisi kullanılarak geliştirilen bir e-ticaret sisteminin **Catalog (Katalog) servisidir**. Ürün yönetimi işlemleri, modern yazılım desenleri ve kütüphaneler kullanılarak gerçekleştirilmiştir.

---

## 🚀 Kullanılan Teknolojiler ve Kütüphaneler

- **.NET 8 / ASP.NET Core**
- **Marten** → PostgreSQL üzerinde Document DB yetenekleri sağlar  
- **MediatR** → CQRS (Command Query Responsibility Segregation) deseni için  
- **Carter** → Minimal API endpoint’lerini modüler şekilde tanımlamak için  
- **Mapster** → Yüksek performanslı nesne eşleme (mapping) işlemleri  
- **Docker & Docker Compose** → Konteyner yönetimi  

---

## 🏗️ Mimari Yapı

Projede **CQRS (Command Query Responsibility Segregation)** ve **Vertical Slice Architecture** uygulanmıştır.  
İşlemler ikiye ayrılmıştır:

### 🔍 Queries (Okuma İşlemleri)

- **GetProducts** → Tüm ürünleri listeler  
- **GetProductById** → ID’ye göre ürün getirir  
- **GetProductByCategory** → Kategoriye göre filtreleme yapar  

### ✏️ Commands (Yazma İşlemleri)

- **CreateProduct** → Yeni ürün ekler  
- **UpdateProduct** → Ürünü günceller  
- **DeleteProduct** → Ürünü siler  

---

## 📁 Klasör Yapısı (Vertical Slice)

Bu projede katmanlar (Layer) yerine **özellikler (Feature)** baz alınmıştır.  
Her klasör (`CreateProduct`, `GetProducts` vb.) kendi içinde şunları barındırır:

- **Command / Query** → İş mantığı  
- **Handler** → İşlemi gerçekleştiren yapı  
- **Validator** → FluentValidation ile veri doğrulaması  
- **Endpoint** → API rotası  

---

## 🧩 Cross-Cutting Concerns

- **Logging** → Uygulama loglama mekanizması  
- **Global Exception Handling** → Merkezi hata yönetimi  
- **Validation Pipeline** → FluentValidation ile request doğrulama  

---

## 📡 API Endpoints

- `GET /products`
- `GET /products/{id}`
- `GET /products/category/{category}`
- `POST /products`
- `PUT /products`
- `DELETE /products/{id}`

---

## 🏥 Health Checks

Sistem durumunu kontrol etmek için:

- `http://localhost:6000/health` → API ve veritabanı durumunu JSON formatında döner  

---

## 🛠️ Kurulum ve Çalıştırma

1. **Docker Desktop** çalışır durumda olmalıdır  

2. Terminalde proje dizinine gidin:

```bash
docker-compose up -d
````

3. **Catalog.API** projesini çalıştırın

4. Swagger üzerinden API endpoint’lerini test edebilirsiniz

---

## 📌 Not

Bu proje, **Microservices Architecture**, **CQRS** ve **Vertical Slice Architecture** yaklaşımları temel alınarak geliştirilmiştir.


