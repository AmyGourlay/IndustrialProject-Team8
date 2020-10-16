export { default as Button } from '../..\\components\\Button.vue'
export { default as Card } from '../..\\components\\Card.vue'
export { default as LeaderBoard } from '../..\\components\\LeaderBoard.vue'
export { default as LobbyTable } from '../..\\components\\LobbyTable.vue'
export { default as Logo } from '../..\\components\\Logo.vue'
export { default as QuizzioLbl } from '../..\\components\\QuizzioLbl.vue'

export const LazyButton = import('../..\\components\\Button.vue' /* webpackChunkName: "components_Button" */).then(c => c.default || c)
export const LazyCard = import('../..\\components\\Card.vue' /* webpackChunkName: "components_Card" */).then(c => c.default || c)
export const LazyLeaderBoard = import('../..\\components\\LeaderBoard.vue' /* webpackChunkName: "components_LeaderBoard" */).then(c => c.default || c)
export const LazyLobbyTable = import('../..\\components\\LobbyTable.vue' /* webpackChunkName: "components_LobbyTable" */).then(c => c.default || c)
export const LazyLogo = import('../..\\components\\Logo.vue' /* webpackChunkName: "components_Logo" */).then(c => c.default || c)
export const LazyQuizzioLbl = import('../..\\components\\QuizzioLbl.vue' /* webpackChunkName: "components_QuizzioLbl" */).then(c => c.default || c)
