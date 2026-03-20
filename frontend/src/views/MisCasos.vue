<template>
    <div class="min-h-screen bg-despacho-oscuro p-6 md:p-10 text-despacho-fondo relative font-sans">
      <div class="max-w-7xl mx-auto">
        
        <header class="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-10 pb-6 border-b border-white/10">
          <div>
            <h2 class="text-3xl font-bold tracking-tight text-white">Mis Expedientes</h2>
            <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5 shadow-[0_0_10px_rgba(128,0,0,0.5)]"></div>
            <p class="text-despacho-fondo/60 font-medium mt-1">Consulta el progreso de tus asuntos legales</p>
          </div>
          
          <router-link 
            to="/" 
            class="text-xs font-bold uppercase tracking-widest text-despacho-tierra hover:text-white transition-colors"
          >
            Volver al Inicio
          </router-link>
        </header>
  
        <main>
          <div v-if="cargando" class="flex flex-col items-center justify-center py-20 text-despacho-fondo/20">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-despacho-rojo mb-4"></div>
            <p>Sincronizando tus datos...</p>
          </div>
  
          <div v-else-if="casos.length === 0" class="text-center py-24 bg-white/5 rounded-3xl border border-dashed border-white/10">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1" stroke="currentColor" class="w-16 h-16 mx-auto text-despacho-fondo/20 mb-4">
              <path stroke-linecap="round" stroke-linejoin="round" d="M12 21v-8.25M15.75 21v-8.25M8.25 21v-8.25M3 9l9-6 9 6m-1.5 12V10.332A48.36 48.36 0 0012 9.75c-2.551 0-5.056.2-7.5.582V21M3 21h18M12 6.75h.008v.008H12V6.75z" />
            </svg>
            <h3 class="text-xl font-bold text-despacho-fondo/40">Aún no tienes expedientes activos</h3>
            <p class="text-sm text-despacho-fondo/20 mt-2 max-w-sm mx-auto">Una vez que tu abogado inicie tu proceso, aparecerá aquí toda la información.</p>
          </div>
  
          <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div 
              v-for="caso in casos" 
              :key="caso.id" 
              class="bg-white/5 backdrop-blur-sm rounded-3xl shadow-xl border border-white/5 hover:border-despacho-rojo/30 transition-all p-7 relative overflow-hidden group"
            >
              <div class="absolute left-0 top-0 bottom-0 w-1 bg-despacho-rojo/30 group-hover:bg-despacho-rojo transition-colors"></div>
  
              <div class="mb-6">
                <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra block mb-1">Estatus Actual</span>
                <span class="inline-block bg-despacho-rojo/20 text-despacho-rosa text-[10px] font-black px-3 py-1 rounded-full uppercase border border-despacho-rosa/30">
                  {{ caso.estatus?.descripcion || 'En Revisión' }}
                </span>
              </div>
  
              <h3 class="text-xl font-bold text-white mb-4 leading-tight">{{ caso.titulo }}</h3>
  
              <div class="space-y-3 text-sm mb-8">
                <div class="flex items-center gap-3">
                  <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra">Rama:</span>
                  <span class="text-despacho-fondo/80">{{ caso.ramaJuridica?.descripcion }}</span>
                </div>
              </div>
  
              <button 
                @click="router.push(`/caso/${caso.id}`)"
                class="w-full py-3 bg-white/5 hover:bg-white/10 border border-white/10 text-white text-xs font-bold uppercase tracking-widest rounded-xl transition-all active:scale-95"
              >
                Ver Detalles del Progreso
              </button>
            </div>
          </div>
        </main>
  
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  import Swal from 'sweetalert2'
  
  const casos = ref([])
  const cargando = ref(true)
  const router = useRouter()
  
  const obtenerMisCasos = async () => {
    const token = localStorage.getItem('token')
    if (!token) { router.push('/login'); return }
  
    try {
      const respuesta = await axios.get('http://localhost:5039/api/casos/mis-casos', {
        headers: { Authorization: `Bearer ${token}` }
      })
      casos.value = respuesta.data
    } catch (error) {
      console.error(error)
      Swal.fire({
        icon: 'error',
        title: 'Error de conexión',
        text: 'No pudimos cargar tus expedientes.',
        background: '#1a1a1c',
        color: '#fdfbd4'
      })
    } finally {
      cargando.value = false
    }
  }
  
  onMounted(obtenerMisCasos)
  </script>