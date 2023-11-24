<h1 align="center">
    🌿  VKanave 🌿
</h1>

<h2 align="center">
<b>Добро пожаловать в прекрасную ВКанаву.</b>
</h2>

| Авторизация | Чаты | Всплывающее меню | Чатик |
|--|--|--|--|
| <img src="Assets/p3.png" alt="image" width="300" height="450"> | <img src="Assets/chats.png" alt="image" width="300" height="450"> | <img src="Assets/p2.png" alt="image" width="300" height="450"> | <img src="Assets/chat1.png" alt="image" width="300" height="450"> |

<p align="center">
<img src="Assets/p1.png" width="300" height="auto">
</p>P

# Компиляция

### Android (client)

```sh
dotnet publish -c release -r ubuntu.20.04-x64
```
[.bat file](VKanave/compile.bat)

### Ubuntu 20.04-x64 (server)

```sh
dotnet publish -c release -f:net8.0-android
```
[.bat file](VKanaveServer/compile.bat)

# Запуск сервера

> [!NOTE] 
> Установка на Ubuntu.20.04-x64
> 
Устанавливаем mysql-server

```bash
sudo apt install mysql-server
```

Устанавливаем и настраиваем apache и phpmyadmin. Открываем порты и выдаём права доступа программе.

```shell
sudo apt install vsftpd
sudo ufw allow 20,21,990/tcp
sudo ufw allow 40000:50000/tcp
sudo ufw enable
sudo chmod 777 VKanave
```

Запускаем (с использованием мультиплексора)

```shell
tmux new -s vkanave
./VKanave
```

## Результат:

![](Assets/s1.png)

![](Assets/s2.png)
