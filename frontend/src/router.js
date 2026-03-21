import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'
import Login from './views/Login.vue'
import Registro from './views/Registro.vue'
import Casos from './views/Casos.vue'
import Citas from './views/Citas.vue'
import CasoDetalle from './views/CasoDetalle.vue'
import MisCasos from './views/MisCasos.vue'
import Dashboard from './views/Dashboard.vue'

const obtenerRolToken = () => {
    const token = localStorage.getItem('token')
    if (!token) return null
    try {
      const base64Url = token.split('.')[1]
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
      const payload = JSON.parse(window.atob(base64))
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    } catch {
      return null
    }
  }
  
  const routes = [
    { path: '/', name: 'Home', component: Home },
    { path: '/login', name: 'Login', component: Login },
    { path: '/registro', name: 'Registro', component: Registro },
    { path: '/casos', name: 'Casos', component: Casos, meta: { requiresAuth: true, roles: ['Abogado', 'Administrador'] } },
    { path: '/citas', name: 'Citas', component: Citas,meta: { requiresAuth: true, roles: ['Abogado', 'Administrador'] } },
    { path: '/mis-casos', name: 'MisCasos', component: MisCasos,meta: { requiresAuth: true, roles: ['Cliente', 'Administrador'] } },
    { path: '/caso/:id', name: 'CasoDetalle', component: CasoDetalle,meta: { requiresAuth: true } },
    { path: '/dashboard', name: 'Dashboard', component: Dashboard, meta: { requiresAuth: true, roles: ['Abogado', 'Administrador'] } },
  ]
  
  const router = createRouter({
    history: createWebHistory(),
    routes
  })
  
  router.beforeEach((to, from, next) => {
    const isLogged = !!localStorage.getItem('token')
    const userRole = obtenerRolToken()
    if (to.meta.requiresAuth && !isLogged) {
      return next('/login')
    }
    if (to.meta.roles && !to.meta.roles.includes(userRole)) {
      return next('/')
    }
    next()
  })
  
  export default router