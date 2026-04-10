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
- **Scrutor** → Decorator Pattern ile dependency injection desteği  
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
- Redis ile yüksek performanslı cache yönetimi  
- Cache-Aside Pattern implementasyonu  

---

## 🏗️ Mimari Yapı

Projede aşağıdaki modern mimari yaklaşımlar uygulanmıştır:

- **Microservices Architecture**
- **CQRS (Command Query Responsibility Segregation)**
- **Vertical Slice Architecture**
- **Decorator Pattern**
- **Cache-Aside Pattern**

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
```
3. API projelerini çalıştırın

4. API’leri test edin:

### 🔹 Swagger UI

* Catalog API → http://localhost:6000/swagger
* Basket API → http://localhost:6001/swagger

### 🔹 Postman

* Endpoint’ler Postman üzerinden de test edilebilir

> Not: Portlar proje ayarlarına göre değişebilir.





