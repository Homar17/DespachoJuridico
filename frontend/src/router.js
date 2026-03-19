import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'
import Login from './views/Login.vue'
import Registro from './views/Registro.vue'
import Casos from './views/Casos.vue'
import Citas from './views/Citas.vue' // <-- 1. Importar la vista

const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/login', name: 'Login', component: Login },
  { path: '/registro', name: 'Registro', component: Registro },
  { path: '/casos', name: 'Casos', component: Casos },
  { path: '/citas', name: 'Citas', component: Citas } // <-- 2. Registrar la ruta
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router