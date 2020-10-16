import Vue from 'vue'
import Router from 'vue-router'
import lobby from '@/pages/lobby'

Vue.use(Router)

export default new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [
        {
            path: '/',
            name: 'lobby',
            component: lobby
        },
        {
            path: '/game',
            name: 'game',
            component: () => import('./pages/game.vue')
        }
    ]
})