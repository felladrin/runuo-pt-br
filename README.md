# RunUO em Porutuguês do Brasil

[![Join the chat at https://gitter.im/runuo/runuo](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/runuo/runuo?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Este é um fork do [RunUO 2.6](https://github.com/runuo/runuo/tree/releases/2.6) que visa traduzir todas as strings do servidor, sem alterar nenhuma funcionalidade do emulador.

![](http://i.imgur.com/mccrlR0.png)

## Rodando o Servidor no Windows

1. [Baixe este repositório](https://github.com/felladrin/last-wish/archive/master.zip) ou clone-o.
2. Rode `CompilarParaWindows.bat` para gerar os arquivos *RunUO.exe* e *RunUO.exe.config* na pasta.
3. Então execute RunUO.exe

## Rodando o Servidor no Ubuntu

Aqui estão todos os comandos que você precisa executar, em ordem, para ter o servidor pronto para uso:

    apt-get update
    apt-get install mono-complete git
    git clone --depth 1 https://github.com/felladrin/runuo-pt-br.git
    cd runuo-pt-br
    mcs -optimize+ -unsafe -t:exe -out:RunUO.exe -win32icon:Server/runuo.ico -nowarn:219,414 -d:MONO -recurse:Server/*.cs
    cp RunUO.exe.config.Linux RunUO.exe.config
    chmod +x cron.sh
    mono RunUO.exe

Se você deseja rodá-lo manualmente, como uma tarefa rodando em segundo plano, digite `nohup mono RunUO.exe >> console.log &`. E depois, antes de fechar o terminal, digite: `disown` para desvincular o processo do terminal. Porém, melhor do que fazer a execução manual é configurar um trabalho cron para executar o arquivo `cron.sh` periodicamente (por exemplo, a cada 3 minutos). O cron.sh é um simples script que verifica se RunUO.exe já está em execução, e se não estiver, ele o inicializa.