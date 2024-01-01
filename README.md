# Advent of Code 2023
My solutions to the Advent of Code puzzles for the 2023 edition, written in C#.

<style>
    .heatMap th { background: grey; color: white; }
    
    .heatMap tr:nth-child(1) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(1) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(2) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(2) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(3) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(3) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(4) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(4) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(5) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(5) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(6) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(6) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(7) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(7) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(8) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(8) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(9) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(9) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(10) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(10) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(11) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(11) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(12) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(12) td:nth-child(3) { background: #ff4747!important; }
    
    .heatMap tr:nth-child(13) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(13) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(14) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(14) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(15) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(15) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(16) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(16) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(17) td:nth-child(2) { background: none; }
    .heatMap tr:nth-child(17) td:nth-child(3) { background: none; }
    
    .heatMap tr:nth-child(18) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(18) td:nth-child(3) { background: #ffd966!important; }
    
    .heatMap tr:nth-child(19) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(19) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(20) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(20) td:nth-child(3) { background: #ffd966!important; }
    
    .heatMap tr:nth-child(21) td:nth-child(2) { background: none; }
    .heatMap tr:nth-child(21) td:nth-child(3) { background: none; }
    
    .heatMap tr:nth-child(22) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(22) td:nth-child(3) { background: #6ed56e!important; }
    
    .heatMap tr:nth-child(23) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(23) td:nth-child(3) { background: #ffd966!important; }
    
    .heatMap tr:nth-child(24) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(24) td:nth-child(3) { background: #ff9e42!important; }
    
    .heatMap tr:nth-child(25) td:nth-child(2) { background: #6ed56e!important; }
    .heatMap tr:nth-child(25) td:nth-child(3) { background: none; }
    
    .legend th { display: none; }
    
    .legend tr:nth-child(1) td:nth-child(1) { background: #6ed56e!important; }
    .legend tr:nth-child(2) td:nth-child(1) { background: #ffd966!important; }
    .legend tr:nth-child(3) td:nth-child(1) { background: #ff9e42!important; }
    .legend tr:nth-child(4) td:nth-child(1) { background: #ff4747!important; }
</style>

<div class="heatMap">

| AoC Puzzle | Part one | Part two | 
| -- | -- | -- |
| [Day 1: Trebuchet?!](https://github.com/robhabraken/advent-of-code-2023/tree/main/01) | 1.1 | 1.2 |
| [Day 2: Cube Conundrum](https://github.com/robhabraken/advent-of-code-2023/tree/main/02) | 2.1 | 2.2 |
| [Day 3: Gear Ratios](https://github.com/robhabraken/advent-of-code-2023/tree/main/03) | 3.1 | 3.2 |
| [Day 4: Scratchcards](https://github.com/robhabraken/advent-of-code-2023/tree/main/04) | 4.1 | 4.2 |
| [Day 5: If You Give A Seed A Fertilizer](https://github.com/robhabraken/advent-of-code-2023/tree/main/05) | 5.1 | 5.2 |
| [Day 6: Wait For It](https://github.com/robhabraken/advent-of-code-2023/tree/main/06) | 6.1 | 6.2 |
| [Day 7: Camel Cards](https://github.com/robhabraken/advent-of-code-2023/tree/main/07) | 7.1 | 7.2 |
| [Day 8: Haunted Wasteland](https://github.com/robhabraken/advent-of-code-2023/tree/main/08) | 8.1 | 8.2 |
| [Day 9: Mirage Maintenance](https://github.com/robhabraken/advent-of-code-2023/tree/main/09) | 9.1 | 9.2 |
| [Day 10: Pipe Maze](https://github.com/robhabraken/advent-of-code-2023/tree/main/10) | 10.1 | 10.2 |
| [Day 11: Cosmic Expansion](https://github.com/robhabraken/advent-of-code-2023/tree/main/11) | 11.1 | 11.2 |
| [Day 12: Hot Springs](https://github.com/robhabraken/advent-of-code-2023/tree/main/12) | 12.1 | 12.2 |
| [Day 13: Point of Incidence](https://github.com/robhabraken/advent-of-code-2023/tree/main/13) | 13.1 | 13.2 |
| [Day 14: Parabolic Reflector Dish](https://github.com/robhabraken/advent-of-code-2023/tree/main/14) | 14.1 | 14.2 |
| [Day 15: Lens Library](https://github.com/robhabraken/advent-of-code-2023/tree/main/15) | 15.1 | 15.2 |
| [Day 16: The Floor Will Be Lava](https://github.com/robhabraken/advent-of-code-2023/tree/main/16) | 16.1 | 16.2 |
| Day 17: Clumsy Crucible | *In progress* | *Not started* |
| [Day 18: Lavaduct Lagoon](https://github.com/robhabraken/advent-of-code-2023/tree/main/18) | 18.1 | 18.2 |
| [Day 19: Aplenty](https://github.com/robhabraken/advent-of-code-2023/tree/main/19) | 19.1 | 19.2 |
| [Day 20: Pulse Propagation](https://github.com/robhabraken/advent-of-code-2023/tree/main/20) | 20.1 | 20.2 |
| Day 21: Step Counter | *In progress* | *Not started* |
| [Day 22: Sand Slabs](https://github.com/robhabraken/advent-of-code-2023/tree/main/22) | 22.1 | 22.2 |
| [Day 23: A Long Walk](https://github.com/robhabraken/advent-of-code-2023/tree/main/23) | 23.1 | 23.2 |
| [Day 24: Never Tell Me The Odds](https://github.com/robhabraken/advent-of-code-2023/tree/main/24) | 24.1 | 24.2 |
| [Day 25: Snowverload](https://github.com/robhabraken/advent-of-code-2023/tree/main/25) | 25.1 | *Not started* |

</div>
<div class="legend">

### Color legend

| | | 
| -- | -- |
| Green | Solved completely by myself without any help or external input |
| Yellow | Searched online for algorithms or inspiration to solve this problem |
| Orange | Own implementation based on approach as seen on AoC subreddit |
| Red | Wasn't able to solve this myself, implemented someone else's logic to learn from |

</div>