@echo off
chcp 65001 >nul
cd /d "C:\UnityProjects\universal 3d"

echo ============================================
echo  Subindo o MinigameArena pro GitHub
echo ============================================
echo.
echo ANTES de continuar:
echo  1) Entre em github.com e crie um repositorio VAZIO
echo     (NAO marque "Add a README", nem .gitignore, nem license)
echo  2) Copie a URL que aparece, terminada em .git
echo     Ex: https://github.com/Pietro-Piccoli/MinigameArena.git
echo.
set /p REPO=Cole a URL do repositorio e aperte Enter:

echo.
echo Subindo...
if not exist ".git" git init
git add .
git commit -m "Minigame Arena - Design Patterns Factory e Strategy"
git branch -M main
git remote remove origin 2>nul
git remote add origin %REPO%
git push -u origin main

echo.
echo ============================================
echo  PRONTO! Link pra entregar ao professor:
echo  %REPO%
echo ============================================
pause
