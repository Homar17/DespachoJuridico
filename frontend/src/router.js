import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'
import Login from './views/Login.vue'
import Registro from './views/Registro.vue'
import Casos from './views/Casos.vue'
import Citas from './views/Citas.vue'
import CasoDetalle from './views/CasoDetalle.vue'

const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/login', name: 'Login', component: Login },
  { path: '/registro', name: 'Registro', component: Registro },
  { path: '/casos', name: 'Casos', component: Casos },
  { path: '/citas', name: 'Citas', component: Citas },
  { path: '/caso/:id', name: 'CasoDetalle', component: CasoDetalle }, 
  { path: '/:pathMatch(.*)*', redirect: '/' } 
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router