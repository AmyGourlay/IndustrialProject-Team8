import Vue from 'vue'
import Router from 'vue-router'
import lobby from '@/pages/lobby'
import home from '@/pages/index'

Vue.use(Router)

export default new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    routes: [
        {
            path: '/',
            name: 'home',
            component: home
        },
        {
          path: '/lobby/:playerDeets',
          name: 'lobby',
          component: lobby
        },
        {
            path: '/game/:playerInfo',
            name: 'game',
            component: () => import('./pages/game.vue'),
            params: true
        }
    ]
})
