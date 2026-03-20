<template>
    <div class="min-h-screen flex items-center justify-center bg-despacho-oscuro p-4 font-sans">
      <div class="bg-white/5 backdrop-blur-md p-8 md:p-10 rounded-3xl shadow-2xl border border-white/10 w-full max-w-md">
        
        <div class="text-center mb-8">
          <h2 class="text-3xl font-bold tracking-tight mb-2 text-despacho-fondo">Crear Cuenta</h2>
          <div class="h-1 w-16 bg-despacho-rojo mx-auto rounded-full mb-3 shadow-[0_0_15px_rgba(128,0,0,0.6)]"></div>
          <p class="text-despacho-fondo/50 text-sm">Regístrate para gestionar tus expedientes</p>
        </div>
  
        <form @submit.prevent="hacerRegistro" class="space-y-4">
          
          <div class="text-left">
            <label class="block text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Nombre Completo</label>
            <input 
              type="text" 
              v-model="nombre" 
              placeholder="Juan Pérez" 
              class="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
          </div>
  
          <div class="text-left">
            <label class="block text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Teléfono</label>
            <input 
              type="tel" 
              v-model="telefono" 
              placeholder="33 1234 5678" 
              class="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
          </div>
  
          <div class="text-left">
            <label class="block text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Correo Electrónico</label>
            <input 
              type="email" 
              v-model="correo" 
              placeholder="juan@ejemplo.com" 
              class="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
          </div>
  
          <div class="text-left">
            <label class="block text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-1.5 ml-1">Contraseña</label>
            <input 
              type="password" 
              v-model="contrasena" 
              placeholder="••••••••" 
              class="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-despacho-fondo placeholder:text-despacho-fondo/20 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
              required 
            />
            
            <div v-if="contrasena.length > 0" class="mt-3 bg-black/20 p-4 rounded-xl border border-white/5 space-y-2 transition-all">
              <p class="font-bold text-despacho-tierra uppercase tracking-widest text-[9px] mb-1">Seguridad de la cuenta:</p>
              <ul class="grid grid-cols-1 gap-1.5">
                <li :class="validaciones.longitud ? 'text-green-400' : 'text-despacho-fondo/30'" class="flex items-center gap-2 text-[11px] transition-colors">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" class="w-3.5 h-3.5">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.857-9.809a.75.75 0 00-1.214-.882l-3.483 4.79-1.88-1.88a.75.75 0 10-1.06 1.061l2.5 2.5a.75.75 0 001.137-.089l4-5.5z" clip-rule="evenodd" />
                  </svg>
                  8+ caracteres
                </li>
                <li :class="validaciones.mayuscula ? 'text-green-400' : 'text-despacho-fondo/30'" class="flex items-center gap-2 text-[11px] transition-colors">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" class="w-3.5 h-3.5">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.857-9.809a.75.75 0 00-1.214-.882l-3.483 4.79-1.88-1.88a.75.75 0 10-1.06 1.061l2.5 2.5a.75.75 0 001.137-.089l4-5.5z" clip-rule="evenodd" />
                  </svg>
                  Una Mayúscula
                </li>
                <li :class="validaciones.numero ? 'text-green-400' : 'text-despacho-fondo/30'" class="flex items-center gap-2 text-[11px] transition-colors">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" class="w-3.5 h-3.5">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.857-9.809a.75.75 0 00-1.214-.882l-3.483 4.79-1.88-1.88a.75.75 0 10-1.06 1.061l2.5 2.5a.75.75 0 001.137-.089l4-5.5z" clip-rule="evenodd" />
                  </svg>
                  Un Número
                </li>
                <li :class="validaciones.especial ? 'text-green-400' : 'text-despacho-fondo/30'" class="flex items-center gap-2 text-[11px] transition-colors">
                  <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" class="w-3.5 h-3.5">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.857-9.809a.75.75 0 00-1.214-.882l-3.483 4.79-1.88-1.88a.75.75 0 10-1.06 1.061l2.5 2.5a.75.75 0 001.137-.089l4-5.5z" clip-rule="evenodd" />
                  </svg>
                  Símbolo (!@#$)
                </li>
              </ul>
            </div>
          </div>
  
          <button 
            type="submit" 
            :disabled="!formularioValido"
            :class="[
              'w-full py-4 mt-4 font-bold rounded-xl shadow-lg transition-all tracking-wider uppercase text-sm',
              formularioValido 
                ? 'bg-despacho-rojo hover:bg-despacho-rojo/90 text-white shadow-black/40 active:scale-[0.97]' 
                : 'bg-white/10 text-white/20 cursor-not-allowed border border-white/5'
            ]"
          >
            Registrarme Ahora
          </button>
        </form>
  
        <p class="mt-8 text-center text-sm text-despacho-fondo/40">
          ¿Ya eres parte del despacho? 
          <router-link to="/login" class="text-despacho-rojo hover:text-despacho-rosa transition-colors font-bold ml-1">Inicia Sesión</router-link>
        </p>
  
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  import Swal from 'sweetalert2'
  
  const nombre = ref('')
  const telefono = ref('')
  const correo = ref('')
  const contrasena = ref('')
  const router = useRouter()
  
  const validaciones = computed(() => {
    return {
      longitud: contrasena.value.length >= 8,
      mayuscula: /[A-Z]/.test(contrasena.value),
      numero: /\d/.test(contrasena.value),
      especial: /[\W_]/.test(contrasena.value)
    }
  })
  
  const formularioValido = computed(() => {
    return nombre.value.trim() !== '' && 
           telefono.value.trim() !== '' &&
           correo.value.trim() !== '' &&
           validaciones.value.longitud && 
           validaciones.value.mayuscula && 
           validaciones.value.numero && 
           validaciones.value.especial
  })
  
  const hacerRegistro = async () => {
    if (!formularioValido.value) return 
  
    try {
      await axios.post('http://localhost:5039/api/usuarios', {
        nombre: nombre.value,
        telefono: telefono.value,
        correo: correo.value,
        contraseña: contrasena.value,
        User: correo.value.split('@')[0],
        rolId: 2
      })
  
      Swal.fire({
        icon: 'success',
        title: '¡Registro Exitoso!',
        text: 'Tu cuenta ha sido creada. Ahora puedes iniciar sesión para agendar tu cita.',
        background: '#1a1a1c',
        color: '#fdfbd4',
        confirmButtonColor: '#8B0000',
        timer: 3000,
        timerProgressBar: true
      })
  
      setTimeout(() => {
        router.push('/login')
      }, 1500)
      
    } catch (err) {
      console.error(err)
      
      const errorMsg = err.response?.data || "Ocurrió un error inesperado al crear tu cuenta."
      
      Swal.fire({
        icon: 'error',
        title: 'Error al registrar',
        text: typeof errorMsg === 'string' ? errorMsg : "Verifica los datos e intenta de nuevo.",
        background: '#1a1a1c',
        color: '#fdfbd4',
        confirmButtonColor: '#8B0000'
      })
    }
  }
  </script>