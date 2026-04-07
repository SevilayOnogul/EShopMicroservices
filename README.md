````md
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

Projede **CQRS (Command Query Responsibility Segregation)** deseni uygulanmıştır.  
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

Bu proje, **Microservices Architecture**, **CQRS** ve **Vertical Slice yaklaşımı** temel alınarak geliştirilmiştir.

```
```
