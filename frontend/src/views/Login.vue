<template>
    <div class="min-h-screen flex items-center justify-center bg-despacho-oscuro p-4">
      
      <div class="bg-white/5 backdrop-blur-md p-8 md:p-10 rounded-3xl shadow-2xl border border-white/10 w-full max-w-md">
        
        <div class="text-center mb-10">
          <h2 class="text-3xl font-bold tracking-tight mb-2 text-despacho-fondo">Despacho Jurídico</h2>
          <div class="h-1 w-20 bg-despacho-rojo mx-auto rounded-full mb-3 shadow-[0_0_15px_rgba(128,0,0,0.6)]"></div>
          <p class="text-despacho-fondo/50 text-sm">Ingresa tus credenciales para continuar al panel</p>
        </div>
  
        <form @submit.prevent="hacerLogin" class="space-y-6">
          
          <div class="text-left">
            <label class="block text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Correo Electrónico</label>
            <input 
              type="email" 
              v-model="correo" 
              placeholder="admin@despacho.com" 
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
            class="w-full py-4 mt-4 bg-despacho-rojo hover:bg-despacho-rojo/90 text-white font-bold rounded-xl shadow-lg shadow-black/40 transition-all active:scale-[0.97] tracking-wider"
          >
            ENTRAR AL SISTEMA
          </button>
        </form>
  
        <p v-if="mensajeError" class="mt-8 text-center text-sm font-semibold text-red-400 bg-red-950/30 p-3 rounded-xl border border-red-900/50">
          {{ mensajeError }}
        </p>
  
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  
  const correo = ref('')
  const contrasena = ref('')
  const mensajeError = ref('')
  const router = useRouter()
  
  const hacerLogin = async () => {
    mensajeError.value = ''
    
    try {
      const respuesta = await axios.post('http://localhost:5039/api/auth/login', {
        correo: correo.value,
        contraseña: contrasena.value
      })
  
      localStorage.setItem('token', respuesta.data.token)
      router.push('/') 
      
    } catch (error) {
      console.error(error)
      mensajeError.value = "Credenciales incorrectas o error de servidor."
    }
  }
  </script>