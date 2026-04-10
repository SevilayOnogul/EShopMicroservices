# 🛒 EShop Microservices

Bu proje, mikroservis mimarisi kullanılarak geliştirilen bir e-ticaret sistemidir.  
Sistem, farklı iş alanlarına ayrılmış bağımsız servislerden oluşur ve modern yazılım mimarileri ile geliştirilmiştir.

---

## 🚀 Kullanılan Teknolojiler ve Kütüphaneler

- **.NET 8 / ASP.NET Core**
- **Marten** → PostgreSQL üzerinde Document DB yetenekleri sağlar  
- **MediatR** → CQRS (Command Query Responsibility Segregation) deseni için  
- **Carter** → Minimal API endpoint’lerini modüler şekilde tanımlamak için  
- **Mapster** → Yüksek performanslı nesne eşleme (mapping) işlemleri  
- **Redis** → Dağıtık cache yönetimi (Basket Service)  
- **Docker & Docker Compose** → Konteyner yönetimi  

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
- Redis kullanılarak yüksek performanslı cache yönetimi  

---

## 🏗️ Mimari Yapı

Projede aşağıdaki modern mimari yaklaşımlar uygulanmıştır:

- **Microservices Architecture**
- **CQRS (Command Query Responsibility Segregation)**
- **Vertical Slice Architecture**

---

## 📁 Klasör Yapısı (Vertical Slice)

Bu projede katmanlar (Layer) yerine **özellikler (Feature)** baz alınmıştır.  

Her feature klasörü (`CreateProduct`, `GetBasket` vb.) şu yapıyı içerir:

- **Command / Query** → İş mantığı  
- **Handler** → İşlemi gerçekleştiren yapı  
- **Validator** → FluentValidation ile doğrulama  
- **Endpoint** → API rotası  

---

## 🧩 Cross-Cutting Concerns

- **Logging** → Uygulama loglama  
- **Global Exception Handling** → Merkezi hata yönetimi  
- **Validation Pipeline** → FluentValidation entegrasyonu  

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

4. Swagger üzerinden endpoint’leri test edin

---

## 📌 Not

Bu proje, **Microservices Architecture**, **CQRS** ve **Vertical Slice Architecture** yaklaşımları temel alınarak geliştirilmiştir.


