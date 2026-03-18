import { createRouter, createWebHistory } from 'vue-router'
import Login from './views/Login.vue'
import Casos from './views/Casos.vue'

const routes = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/casos',
    name: 'Casos',
    component: Casos
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router