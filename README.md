### INFORMATION: Манжосова София Александровна ИП-24-4
#### ТГ: (@Sofishokc) - https://t.me/Sofishokc
#### upd:31.07.2026
- - - - 
# Название проекта: Transfer-OfficeFurniture-Equipment
### Тема: Акт приёма-передачи 
- - - -
## Описание: Автоматизация акта приёма-передачи закупки офисной мебели и оргтехники (для компаний).
#### Статус: Работа с кодом. Создание сущностей. 

Форма акта:
<img width="1080" height="818" alt="Screenshot_20260728_161720" src="https://github.com/user-attachments/assets/def65619-e5ca-43e7-be45-fc5e51d70b05" />

Схема связи сущностей:
```mermaid
erDiagram
    Supplier ||--o{ Contract : "заключает как Продавец"
    Client ||--o{ Contract : "заключает как Покупатель"
    Contract ||--o{ TransferAct : "является основанием для"
    TransferAct ||--o{ TransferActItem : "содержит строки таблицы"

    Supplier {
        int Id PK
        string CompanyName
        string ContactLastName
        string ContactName
        string ContactPatronymic
        string CeoBasis
        string Inn
        string LegalAddress
        string Email
        string Phone
        bool IsActive
    }

    Client {
        int Id PK
        string CompanyName
        string ContactLastName
        string ContactName
        string ContactPatronymic
        string CeoBasis
        string Inn
        string LegalAddress
        string Email
        string Phone
        bool IsActive
    }

    Contract {
        int Id PK
        int SupplierId FK
        int ClientId FK
        string Number
        DateTimeOffset SignedAt
        ContractType Type
    }

    TransferAct {
        int Id PK
        int ContractId FK
        string Number
        DateOnly Date
        string City
    }

    TransferActItem {
        int Id PK
        int TransferActId FK
        string Name
        string StockNumber
        string Specifications
        string Unit
        decimal Quantity
        decimal UnitPrice
    }

    Product {
        int Id PK
        string Name
        string StockNumber
        string Specifications
        string Condition
        string Unit
        decimal Price
    }
```

```
