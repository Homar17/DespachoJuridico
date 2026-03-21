<template>
    <div class="min-h-screen bg-despacho-oscuro p-6 md:p-10 text-despacho-fondo relative font-sans">
      <div class="max-w-7xl mx-auto">
        
        <header class="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-10 pb-6 border-b border-white/10">
          <div>
            <h2 class="text-3xl font-bold tracking-tight text-white">Dashboard Estadístico</h2>
            <div class="h-1 w-16 bg-despacho-rojo rounded-full mt-1.5 shadow-[0_0_10px_rgba(128,0,0,0.5)]"></div>
            <p class="text-despacho-fondo/60 font-medium mt-1">Resumen general del despacho</p>
          </div>
          
          <div class="flex items-center gap-3">
            <router-link to="/" class="text-xs font-bold uppercase tracking-widest text-despacho-tierra hover:text-white transition-colors">
              Ir al Inicio
            </router-link>
            <router-link to="/casos" class="px-5 py-2.5 bg-white/5 border border-white/10 text-despacho-fondo font-semibold rounded-xl hover:bg-white/10 transition-all active:scale-95 text-sm">
              Ver Expedientes
            </router-link>
          </div>
        </header>
  
        <div v-if="cargando" class="flex flex-col items-center justify-center py-32 text-despacho-fondo/20">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-despacho-rojo mb-4"></div>
          <p class="font-medium tracking-widest uppercase text-xs">Calculando métricas...</p>
        </div>
  
        <main v-else class="space-y-8">
          
          <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div class="bg-white/5 p-6 rounded-3xl border border-white/10 shadow-xl relative overflow-hidden">
              <div class="absolute -right-4 -top-4 w-24 h-24 bg-despacho-rojo/10 rounded-full blur-xl"></div>
              <h3 class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-2">Total de Expedientes</h3>
              <p class="text-5xl font-black text-white">{{ estadisticas.totalCasos }}</p>
            </div>
            
            <div class="bg-white/5 p-6 rounded-3xl border border-white/10 shadow-xl relative overflow-hidden">
              <div class="absolute -right-4 -top-4 w-24 h-24 bg-blue-500/10 rounded-full blur-xl"></div>
              <h3 class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-2">Citas Próximas</h3>
              <p class="text-5xl font-black text-white">{{ estadisticas.proximasCitas.length }}</p>
            </div>
            
            <div class="bg-white/5 p-6 rounded-3xl border border-white/10 shadow-xl relative overflow-hidden flex flex-col justify-center items-center text-center">
              <button @click="obtenerEstadisticas" class="p-3 bg-despacho-rojo/20 text-despacho-rojo rounded-full hover:bg-despacho-rojo hover:text-white transition-all active:scale-95">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-8 h-8">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0l3.181 3.183a8.25 8.25 0 0013.803-3.7M4.031 9.865a8.25 8.25 0 0113.803-3.7l3.181 3.182m0-4.991v4.99" />
                </svg>
              </button>
              <span class="text-[10px] font-bold uppercase tracking-widest text-despacho-fondo/50 mt-2">Actualizar Datos</span>
            </div>
          </div>
  
          <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
            
            <div class="bg-black/20 p-8 rounded-3xl border border-dashed border-white/10">
              <h3 class="font-bold text-white uppercase tracking-widest text-sm mb-6 flex items-center gap-2">
                <span class="w-2 h-2 rounded-full bg-despacho-rojo"></span>
                Expedientes por Estatus
              </h3>
              
              <div class="space-y-5">
                <div v-for="item in estadisticas.casosPorEstatus" :key="item.nombre">
                  <div class="flex justify-between text-xs mb-1">
                    <span class="text-despacho-fondo font-medium">{{ item.nombre }}</span>
                    <span class="text-despacho-tierra font-bold">{{ item.cantidad }}</span>
                  </div>
                  <div class="w-full bg-white/5 rounded-full h-2">
                    <div 
                      class="bg-despacho-rojo h-2 rounded-full shadow-[0_0_10px_rgba(128,0,0,0.8)]" 
                      :style="`width: ${calcularPorcentaje(item.cantidad)}%`"
                    ></div>
                  </div>
                </div>
                <div v-if="estadisticas.casosPorEstatus.length === 0" class="text-xs text-despacho-fondo/40 italic">No hay datos suficientes.</div>
              </div>
            </div>
  
            <div class="bg-black/20 p-8 rounded-3xl border border-dashed border-white/10">
              <h3 class="font-bold text-white uppercase tracking-widest text-sm mb-6 flex items-center gap-2">
                <span class="w-2 h-2 rounded-full bg-despacho-tierra"></span>
                Expedientes por Rama Jurídica
              </h3>
              
              <div class="space-y-5">
                <div v-for="item in estadisticas.casosPorRama" :key="item.nombre">
                  <div class="flex justify-between text-xs mb-1">
                    <span class="text-despacho-fondo font-medium">{{ item.nombre }}</span>
                    <span class="text-despacho-tierra font-bold">{{ item.cantidad }}</span>
                  </div>
                  <div class="w-full bg-white/5 rounded-full h-2">
                    <div 
                      class="bg-despacho-tierra h-2 rounded-full" 
                      :style="`width: ${calcularPorcentaje(item.cantidad)}%`"
                    ></div>
                  </div>
                </div>
                <div v-if="estadisticas.casosPorRama.length === 0" class="text-xs text-despacho-fondo/40 italic">No hay datos suficientes.</div>
              </div>
            </div>
  
          </div>
  
          <div class="bg-white/5 p-8 rounded-3xl border border-white/10 shadow-xl">
            <div class="flex justify-between items-center mb-6">
              <h3 class="font-bold text-white uppercase tracking-widest text-sm">Próximas 5 Citas</h3>
              <router-link to="/citas" class="text-[10px] font-bold text-despacho-rojo hover:text-white transition-colors uppercase tracking-widest">
                Ver Agenda Completa &rarr;
              </router-link>
            </div>
  
            <div v-if="estadisticas.proximasCitas.length === 0" class="text-center py-6 text-sm text-despacho-fondo/40 italic border border-dashed border-white/5 rounded-xl">
              No tienes citas agendadas próximamente.
            </div>
  
            <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              <div 
                v-for="cita in estadisticas.proximasCitas" 
                :key="cita.id" 
                class="bg-black/30 p-4 rounded-2xl border border-white/5 flex flex-col justify-between"
              >
                <div>
                  <p class="text-despacho-rojo font-black text-sm mb-1 capitalize">{{ formatearFecha(cita.fechaHora) }}</p>
                  <p class="text-white font-bold mb-2">{{ cita.cliente }}</p>
                  <p class="text-xs text-despacho-fondo/60 line-clamp-2">{{ cita.descripcion }}</p>
                </div>
                <div class="mt-4 pt-3 border-t border-white/5 text-[10px] font-mono text-despacho-tierra">
                  {{ formatearHora(cita.fechaHora) }}
                </div>
              </div>
            </div>
          </div>
  
        </main>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRouter } from 'vue-router'
  import axios from 'axios'
  import Swal from 'sweetalert2'
  
  const router = useRouter()
  const cargando = ref(true)
  const estadisticas = ref({
    totalCasos: 0,
    casosPorEstatus: [],
    casosPorRama: [],
    proximasCitas: []
  })
  
  const obtenerEstadisticas = async () => {
    cargando.value = true
    const token = localStorage.getItem('token')
    if (!token) { router.push('/login'); return }
  
    try {
      const respuesta = await axios.get('http://localhost:5039/api/casos/estadisticas', {
        headers: { Authorization: `Bearer ${token}` }
      })
      estadisticas.value = respuesta.data
    } catch (error) {
      console.error(error)
      if (error.response?.status === 403) {
        router.push('/')
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Error de servidor',
          text: 'No pudimos cargar las estadísticas.',
          background: '#1a1a1c',
          color: '#fdfbd4',
          confirmButtonColor: '#8B0000'
        })
      }
    } finally {
      cargando.value = false
    }
  }
  
  const calcularPorcentaje = (cantidad) => {
    if (estadisticas.value.totalCasos === 0) return 0
    return (cantidad / estadisticas.value.totalCasos) * 100
  }
  
  const formatearFecha = (fechaIso) => {
    const fecha = new Date(fechaIso)
    return fecha.toLocaleDateString('es-MX', { weekday: 'short', day: 'numeric', month: 'short' })
  }
  
  const formatearHora = (fechaIso) => {
    const fecha = new Date(fechaIso)
    return fecha.toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit' }) + ' HRS'
  }
  
  onMounted(obtenerEstadisticas)
  </script>