<h1 align="center">
    🌿  VKanave 🌿
</h1>

<h2 align="center">
<b>Добро пожаловать в прекрасную ВКанаву.</b>
</h2>

| Авторизация | Создание чата |
|--|--| 
| ![](Assets/auth.gif) | ![](Assets/add.gif) |

| Всплывающее меню | Чаты |
|--|--| 
| <img src="Assets/flyoutMenu.png" width="300" height="auto"> | <img src="Assets/chats.png" width="300" height="auto"> |

![](Assets/chat.gif) 

# Компиляция

### Android (client)

[.bat file](VKanave/compile.bat)

```sh
dotnet publish -c release -r ubuntu.20.04-x64
```
[.bat file](VKanaveServer/compile.bat)

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

![](Assets/server1.png)

![](Assets/server2.png)
