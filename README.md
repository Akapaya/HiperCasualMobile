# 3D Hiper Casual Mobile

Projeto mobile 3D no estilo Hiper Casual desenvolvido em Unity, com foco em desempenho, modularidade e f�cil expans�o de sistemas.

## Sobre o Projeto

Este projeto simula um jogo mobile onde o jogador enfrenta NPCs em um ambiente com movimenta��o, ao passar perto de um NPCs ele empurra-o ativando o ragdoll e indo para a pilha, upgrades que melhoram os status como maiores pilhas de items e velocidade. O jogo foi projetado com diversos managers customizados para centralizar responsabilidades e garantir flexibilidade.

## Mapa Mental sobre o processo de desenvolvimento

https://www.canva.com/design/DAGtQRsAjzc/RNddxxxn8TRllhWSCMR07g/edit?utm_content=DAGtQRsAjzc&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton

## Principais Funcionalidades

- **Controle de Jogador** com divis�o entre Controller e Model
- **Inimigos/NPCs** com movimenta��o via Waypoints
- **Empilhamento de Itens** com l�gica de in�rcia e movimenta��o em arco
- **Sell Area**: sistema de venda autom�tica dos itens coletados
- **Upgrade Area**: upgrades aleat�rios com ScriptableObjects
-  **Sistema de Ragdoll** para NPCs com f�sica leve
- **WayPoint Manager** para movimenta��o inteligente de NPCs
- **Object Pooling** para otimiza��o de performance
- **Joystick Virtual** com anima��es din�micas baseadas em intensidade
- **Update Manager** centralizado (IUpdater, ILateUpdater, IFixedUpdater)
- **Camera Manager** leve com `Transform.position`

## Estrutura Modular (Managers)

| Manager            | Fun��o                                                                 |
|--------------------|------------------------------------------------------------------------|
| Area Manager       | Gerencia �reas como venda e upgrades com sistema de observers          |
| Horde Manager      | Controla gera��o e balanceamento de inimigos com ScriptableObject      |
| Object Pool Manager| Reutiliza objetos com Addressables para economia de performance        |
| Update Manager     | Centraliza chamadas de update evitando m�ltiplas chamadas de C++       |
| WayPoints Manager  | Gera caminhos aleat�rios vis�veis no editor para os NPCs               |

## Anima��es e Visual

- Uso de **Animator Parameters** e **Layers** para anima��es superiores e inferiores independentes
- Redu��o de modelos e acess�rios desnecess�rios para otimiza��o gr�fica
- Visual clean e fluido, ideal para dispositivos mobile

## Refer�ncias

- [In�rcia na movimenta��o](https://www.youtube.com/watch?v=h5BZPXj-bo8)
- [Refer�ncia visual (academia)](https://www.youtube.com/watch?v=72NoCCXuf6Q)