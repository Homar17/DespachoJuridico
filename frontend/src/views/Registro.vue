<template>
    <div class="min-h-screen flex items-center justify-center bg-despacho-oscuro p-4">
      <div class="bg-white/5 backdrop-blur-md p-8 md:p-10 rounded-3xl shadow-2xl border border-white/10 w-full max-w-md">
        
        <div class="text-center mb-10">
          <h2 class="text-3xl font-bold tracking-tight mb-2 text-despacho-fondo">Crear Cuenta</h2>
          <div class="h-1 w-16 bg-despacho-rojo mx-auto rounded-full mb-3 shadow-[0_0_15px_rgba(128,0,0,0.6)]"></div>
          <p class="text-despacho-fondo/50 text-sm">Regístrate para agendar tu asesoría legal</p>
        </div>
  
        <form @submit.prevent="hacerRegistro" class="space-y-5">
          
          <div class="text-left">
            <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Nombre Completo</label>
            <input 
              type="text" 
              v-model="nombre" 
              placeholder="Juan Pérez" 
              class="w-full px-4 py-3.5 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
          </div>
  
          <div class="text-left">
            <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Correo Electrónico</label>
            <input 
              type="email" 
              v-model="correo" 
              placeholder="juan@ejemplo.com" 
              class="w-full px-4 py-3.5 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
          </div>
  
          <div class="text-left">
            <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Contraseña</label>
            <input 
              type="password" 
              v-model="contrasena" 
              placeholder="••••••••" 
              class="w-full px-4 py-3.5 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
          </div>
  
          <button 
            type="submit" 
            class="w-full py-4 mt-2 bg-despacho-rojo hover:bg-despacho-rojo/90 text-white font-bold rounded-xl shadow-lg shadow-black/40 transition-all active:scale-[0.97] tracking-wider uppercase"
          >
            Registrarme
          </button>
        </form>
  
        <p class="mt-6 text-center text-sm text-despacho-fondo/50">
          ¿Ya tienes cuenta? 
          <router-link to="/login" class="text-despacho-rojo hover:underline font-bold">Inicia Sesión</router-link>
        </p>
  
        <p v-if="mensaje" :class="['mt-6 text-center text-sm font-semibold p-3 rounded-xl border', error ? 'text-red-400 bg-red-950/30 border-red-900/50' : 'text-green-400 bg-green-950/30 border-green-900/50']">
          {{ mensaje }}
        </p>
  
      </div>
      
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  
  const nombre = ref('')
  const correo = ref('')
  const contrasena = ref('')
  const mensaje = ref('')
  const error = ref(false)
  const router = useRouter()
  
  const hacerRegistro = async () => {
    mensaje.value = ''
    error.value = false
    
    try {
      await axios.post('http://localhost:5039/api/usuarios', {
        nombre: nombre.value,
        correo: correo.value,
        contraseña: contrasena.value,
        rolId: 2 
      })
  
      mensaje.value = "¡Cuenta creada con éxito! Redirigiendo al login..."
      error.value = false
      
      // Lo mandamos al login después de 2 segundos
      setTimeout(() => {
        router.push('/login')
      }, 2000)
      
    } catch (err) {
      console.error(err)
      error.value = true
      mensaje.value = "Hubo un error al crear la cuenta. Verifica los datos."
    }
  }
  </script>