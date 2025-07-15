# 3D Hiper Casual Mobile

Projeto mobile 3D no estilo Hiper Casual desenvolvido em Unity, com foco em desempenho, modularidade e fácil expansão de sistemas.

## Sobre o Projeto

Este projeto simula um jogo mobile onde o jogador enfrenta NPCs em um ambiente com movimentação, ao passar perto de um NPCs ele empurra-o ativando o ragdoll e indo para a pilha, upgrades que melhoram os status como maiores pilhas de items e velocidade. O jogo foi projetado com diversos managers customizados para centralizar responsabilidades e garantir flexibilidade.

## Mapa Mental sobre o processo de desenvolvimento

https://www.canva.com/design/DAGtQRsAjzc/RNddxxxn8TRllhWSCMR07g/edit?utm_content=DAGtQRsAjzc&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton

## Principais Funcionalidades

- **Controle de Jogador** com divisão entre Controller e Model
- **Inimigos/NPCs** com movimentação via Waypoints
- **Empilhamento de Itens** com lógica de inércia e movimentação em arco
- **Sell Area**: sistema de venda automática dos itens coletados
- **Upgrade Area**: upgrades aleatórios com ScriptableObjects
-  **Sistema de Ragdoll** para NPCs com física leve
- **WayPoint Manager** para movimentação inteligente de NPCs
- **Object Pooling** para otimização de performance
- **Joystick Virtual** com animações dinâmicas baseadas em intensidade
- **Update Manager** centralizado (IUpdater, ILateUpdater, IFixedUpdater)
- **Camera Manager** leve com `Transform.position`

## Estrutura Modular (Managers)

| Manager            | Função                                                                 |
|--------------------|------------------------------------------------------------------------|
| Area Manager       | Gerencia áreas como venda e upgrades com sistema de observers          |
| Horde Manager      | Controla geração e balanceamento de inimigos com ScriptableObject      |
| Object Pool Manager| Reutiliza objetos com Addressables para economia de performance        |
| Update Manager     | Centraliza chamadas de update evitando múltiplas chamadas de C++       |
| WayPoints Manager  | Gera caminhos aleatórios visíveis no editor para os NPCs               |

## Animações e Visual

- Uso de **Animator Parameters** e **Layers** para animações superiores e inferiores independentes
- Redução de modelos e acessórios desnecessários para otimização gráfica
- Visual clean e fluido, ideal para dispositivos mobile

## Referências

- [Inércia na movimentação](https://www.youtube.com/watch?v=h5BZPXj-bo8)
- [Referência visual (academia)](https://www.youtube.com/watch?v=72NoCCXuf6Q)