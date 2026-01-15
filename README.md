
# 📚 BookWorld

BookWorld, kullanıcıların kitap satın alabildiği ve kiralayabildiği,  
ASP.NET Core ve Clean Architecture yaklaşımı ile geliştirilmiş bir web uygulamasıdır.

## 🚀 Özellikler
- Kitap satın alma
- Kitap kiralama ve iade
- Kullanıcıya özel sipariş ve kiralama geçmişi
- JWT tabanlı kimlik doğrulama
- Katmanlı ve sürdürülebilir mimari

## 🏗️ Mimari
Proje Clean Architecture prensiplerine uygun olarak geliştirilmiştir:

- **Domain**: Entity’ler ve iş kuralları
- **Application**: DTO’lar ve servisler
- **Infrastructure**: EF Core, repository’ler, JWT handler
- **API**: RESTful servisler
- **UI**: ASP.NET Core MVC, Razor, Bootstrap

## 🔐 Güvenlik
- JWT Authentication
- HttpOnly Cookie kullanımı
- Authorize attribute ile korunan API endpoint’leri

## 🛠️ Kullanılan Teknolojiler
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT
- AutoMapper
- Bootstrap 5

## 📦 Kurulum
1. Repository’yi klonlayın
2. ConnectionString ayarlarını yapın
3. Migration ve update-database çalıştırın
4. API ve UI projelerini başlatın

## 📄 Lisans
Bu proje eğitim ve demo amaçlı geliştirilmiştir.
