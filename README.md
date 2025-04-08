База данных BankOperations на somee.com

Скрипт таблицы Users с добавлением тестовых пользователей:

CREATE TABLE users (

    UserID INT PRIMARY KEY AUTO_INCREMENT,
    
    UserLogin VARCHAR(50) NOT NULL UNIQUE,
    
    UserPassword VARCHAR(255) NOT NULL,
    
    RoleID INT
    
);


INSERT INTO users (UserLogin, UserPassword, RoleID) VALUES

('admin', 'password123', 1),

('user1', 'mypassword', 2),

('user2', 'securepass', 2),

('guest1', 'guestpass', 3),

('admin2', 'adminpass', 1);

Выполнение тестов в проекте:

![image](https://github.com/user-attachments/assets/d6e38c4c-5683-4537-9b7b-ca74ca1e7591)

