<template>
    <div class="min-h-screen bg-despacho-oscuro p-6 md:p-10 text-despacho-fondo relative font-sans">
      <div class="max-w-5xl mx-auto">
        
        <header class="mb-10">
          <button 
            v-if="!editando"
            @click="router.push('/casos')" 
            class="flex items-center gap-2 text-despacho-tierra hover:text-white transition-colors text-sm font-bold uppercase tracking-widest mb-6 group"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-5 h-5 transform group-hover:-translate-x-1 transition-transform">
              <path stroke-linecap="round" stroke-linejoin="round" d="M10.5 19.5L3 12m0 0l7.5-7.5M3 12h18" />
            </svg>
            Volver a Expedientes
          </button>
  
          <div v-if="!cargando && caso" class="flex flex-col md:flex-row md:items-start justify-between gap-6">
            <div class="flex-1">
              <div class="flex items-center gap-4 mb-2">
                <h2 v-if="!editando" class="text-3xl md:text-4xl font-black tracking-tight text-white">{{ caso.titulo }}</h2>
                <input 
                  v-else
                  v-model="formularioEdicion.titulo"
                  type="text"
                  class="flex-1 text-3xl md:text-4xl font-black tracking-tight text-white bg-white/5 border border-despacho-fondo/20 rounded-xl px-4 py-1 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all"
                />
  
                <span v-if="!editando" class="bg-despacho-rojo/20 text-despacho-rosa text-xs font-black px-4 py-1.5 rounded-full uppercase tracking-tighter border border-despacho-rosa/50 whitespace-nowrap">
                  {{ caso.estatus?.descripcion || 'Pendiente' }}
                </span>
                <div v-else class="relative w-40">
                  <select 
                    v-model.number="formularioEdicion.id_estatus"
                    class="w-full text-xs font-black px-4 py-2 rounded-full uppercase tracking-tighter bg-white/10 border border-despacho-rosa/50 text-despacho-rosa appearance-none cursor-pointer focus:outline-none focus:ring-1 focus:ring-despacho-rojo"
                  >
                    <option v-for="est in listaEstatus" :key="est.id" :value="est.id" class="bg-despacho-oscuro text-white">{{ est.descripcion }}</option>
                  </select>
                  <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-3 text-despacho-rosa">
                    <svg class="h-3 w-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M19 9l-7 7-7-7"></path></svg>
                  </div>
                </div>
              </div>
              <div class="h-1 w-20 bg-despacho-rojo rounded-full mt-2 shadow-[0_0_15px_rgba(128,0,0,0.6)]"></div>
            </div>
  
            <div class="flex gap-3">
              <button 
                v-if="!editando"
                @click="activarEdicion"
                class="px-5 py-2.5 bg-white/5 border border-white/10 text-despacho-fondo font-semibold rounded-xl hover:bg-white/10 transition-all active:scale-95 flex items-center gap-2 group"
              >
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4 text-despacho-tierra group-hover:text-white"><path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L10.582 16.07a4.5 4.5 0 01-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 011.13-1.897l8.932-8.931zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0115.75 21H5.25A2.25 2.25 0 013 18.75V8.25A2.25 2.25 0 015.25 6H10" /></svg>
                Editar Expediente
              </button>
  
              <button 
                v-if="editando"
                @click="cancelarEdicion"
                class="px-5 py-2.5 border border-despacho-fondo/30 text-despacho-fondo/70 font-semibold rounded-xl hover:bg-white/5 hover:text-white transition-all active:scale-95"
              >
                Cancelar
              </button>
              <button 
                v-if="editando"
                @click="guardarCambios"
                :disabled="guardando"
                class="px-5 py-2.5 bg-despacho-rojo text-white font-bold rounded-xl shadow-lg hover:bg-despacho-rojo/90 transition-all active:scale-95 disabled:opacity-50"
              >
                {{ guardando ? 'Guardando...' : 'Confirmar Cambios' }}
              </button>
            </div>
          </div>
        </header>
  
        <div v-if="cargando" class="flex flex-col items-center justify-center py-32 text-despacho-fondo/20">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-despacho-rojo mb-4"></div>
          <p class="font-medium">Abriendo expediente...</p>
        </div>
  
        <main v-else-if="caso" class="grid grid-cols-1 md:grid-cols-3 gap-8">
          
          <div class="md:col-span-2 space-y-8">
            
            <section class="bg-white/5 backdrop-blur-sm p-8 rounded-3xl border border-white/10 shadow-xl relative">
              <div class="absolute top-0 left-10 w-32 h-32 bg-despacho-rojo/5 rounded-full blur-3xl -z-10"></div>
              
              <h3 class="text-xs font-bold uppercase tracking-widest text-despacho-tierra mb-4">Hechos / Descripción del Asunto</h3>
              
              <p v-if="!editando" class="text-despacho-fondo/90 leading-relaxed whitespace-pre-wrap text-lg">
                {{ caso.descripcion || 'Sin descripción registrada en este expediente.' }}
              </p>
              <textarea 
                v-else
                v-model="formularioEdicion.descripcion"
                rows="10"
                class="w-full bg-white/5 border border-despacho-fondo/20 text-despacho-fondo rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-despacho-rojo transition-all placeholder:text-despacho-fondo/20 resize-none text-lg leading-relaxed"
              ></textarea>
            </section>
  
            <section class="bg-black/10 p-8 rounded-3xl border border-dashed border-white/10">
               <div class="flex items-center gap-3 mb-6 pb-4 border-b border-white/5">
                  <div class="p-2.5 bg-white/5 rounded-xl border border-white/10 text-despacho-tierra">
                     <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6"><path stroke-linecap="round" stroke-linejoin="round" d="M12 7.5h1.5m-1.5 3h1.5m-7.5 3h7.5m-7.5 3h7.5m3-9h3.375c.621 0 1.125.504 1.125 1.125V18a2.25 2.25 0 01-2.25 2.25H15.75m-12.75-3h11.25m-12.75-3h11.25m-12.75-3h11.25m12.75-9h3.375c.621 0 1.125.504 1.125 1.125V18a2.25 2.25 0 01-2.25 2.25H15.75m-12.75-3h11.25m-12.75-3h11.25m-12.75-3h11.25m12.75-9H8.25c-.621 0-1.125.504-1.125 1.125v1.5a3.375 3.375 0 003.375 3.375h1.5a1.125 1.125 0 011.125 1.125v1.5a3.375 3.375 0 003.375 3.375h1.5a1.125 1.125 0 011.125 1.125v1.5a3.375 3.375 0 003.375 3.375h1.5a1.125 1.125 0 011.125 1.125V18a2.25 2.25 0 01-2.25 2.25H15.75c-.621 0-1.125-.504-1.125-1.125V4.125a9 9 0 00-9-9z" /></svg>
                  </div>
                  <div>
                     <h3 class="font-bold text-white text-lg">Historial de Actuaciones</h3>
                     <p class="text-sm text-despacho-fondo/50">Movimientos y actualizaciones del expediente.</p>
                  </div>
               </div>
               
               <div v-if="historial.length === 0" class="text-center text-despacho-fondo/40 text-sm py-4 italic">
                 Aún no hay actuaciones registradas en este expediente.
               </div>
  
               <div v-else class="space-y-0">
                 <div 
                   v-for="item in historial" 
                   :key="item.id" 
                   class="flex gap-4 group"
                 >
                    <div class="font-mono text-despacho-tierra text-xs w-20 pt-1 text-right">
                      {{ formatearFechaHistorial(item.fechaHora) }}
                    </div>
                    
                    <div class="flex-1 pb-8 border-l border-white/10 pl-6 relative">
                       <div class="absolute -left-1.5 top-1.5 w-3 h-3 bg-despacho-oscuro border-2 border-despacho-rojo rounded-full group-hover:bg-despacho-rojo transition-colors shadow-[0_0_8px_rgba(128,0,0,0.8)]"></div>
                       <p class="font-bold text-despacho-fondo text-sm">{{ item.descripcion }}</p>
                       <p class="text-[10px] text-despacho-fondo/40 mt-1">{{ new Date(item.fechaHora).toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit' }) }}</p>
                    </div>
                 </div>
               </div>
            </section>
          </div>
  
          <aside class="space-y-6">
            
            <div class="bg-white/5 backdrop-blur-sm p-6 rounded-3xl border border-white/10 shadow-xl relative overflow-hidden">
              <div class="absolute top-0 right-0 w-16 h-16 bg-despacho-rojo/10 rounded-bl-full -z-10"></div>
              <h3 class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-3">Cliente Asignado</h3>
              <div class="flex items-center gap-4">
                <div class="w-12 h-12 rounded-full bg-despacho-oscuro border border-despacho-rojo/30 flex items-center justify-center text-white font-bold text-lg">
                  {{ caso.usuario?.nombre ? caso.usuario.nombre.charAt(0).toUpperCase() : '?' }}
                </div>
                <div class="flex-1 min-w-0">
                  <p class="font-bold text-white text-lg truncate">{{ caso.usuario?.nombre || 'Desconocido' }}</p>
                  <p class="text-xs text-despacho-fondo/50 truncate">{{ caso.usuario?.correo || 'Sin correo' }}</p>
                </div>
              </div>
            </div>
  
            <div class="bg-white/5 backdrop-blur-sm p-6 rounded-3xl border border-white/10 shadow-xl space-y-5">
              <h3 class="text-[10px] font-bold uppercase tracking-widest text-despacho-tierra mb-1">Datos Técnicos</h3>
              
              <div>
                <span class="block text-[10px] font-bold uppercase tracking-widest text-despacho-fondo/40 mb-1">ID Único</span>
                <span class="font-mono text-despacho-fondo/90 bg-white/5 px-3 py-1.5 rounded-lg border border-white/5">#{{ caso.id }}</span>
              </div>
              
              <div>
                <span class="block text-[10px] font-bold uppercase tracking-widest text-despacho-fondo/40 mb-1.5">Rama Jurídica</span>
                <span v-if="!editando" class="font-medium text-white">{{ caso.ramaJuridica?.descripcion || 'No especificada' }}</span>
                <div v-else class="relative">
                  <select 
                    v-model.number="formularioEdicion.id_RamaJuridica"
                    class="w-full bg-white/10 border border-white/10 text-white rounded-lg px-3 py-2 focus:outline-none focus:ring-1 focus:ring-despacho-rojo appearance-none cursor-pointer"
                  >
                    <option v-for="rama in listaRamas" :key="rama.id" :value="rama.id" class="bg-despacho-oscuro text-white">{{ rama.descripcion }}</option>
                  </select>
                  <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-3 text-white">
                    <svg class="h-3 w-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M19 9l-7 7-7-7"></path></svg>
                  </div>
                </div>
              </div>
            </div>
            
            <div v-if="!editando" class="mt-12 pt-6 border-t border-despacho-rojo/20 ">
              <button 
                @click="eliminarCaso"
                class="bg-despacho-rosa w-full py-3 flex items-center justify-center gap-2 text-despacho-rojo hover:text-white hover:bg-despacho-rojo border border-despacho-rojo/30 rounded-xl transition-all font-bold text-xs uppercase tracking-widest active:scale-95"
              >
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
                </svg>
                Eliminar Expediente
              </button>
            </div>
  
          </aside>
  
        </main>
  
        <div v-else class="text-center py-20 border border-dashed border-white/10 rounded-3xl bg-black/10">
          <h2 class="text-2xl font-bold text-despacho-fondo/50">Expediente #{{ route.params.id }} no encontrado</h2>
          <p class="text-despacho-fondo/30 mt-1">Verifique el ID o la conexión al servidor.</p>
          <button @click="router.push('/casos')" class="mt-6 px-5 py-2.5 bg-despacho-rojo text-white font-bold rounded-xl hover:bg-despacho-rojo/90 transition-all">
            Volver a la lista
          </button>
        </div>
  
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, reactive, onMounted } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import axios from 'axios'
  
  const route = useRoute()
  const router = useRouter()
  const caso = ref(null)
  const cargando = ref(true)
  
  const editando = ref(false) 
  const guardando = ref(false) 
  const listaRamas = ref([]) 
  const listaEstatus = ref([]) 
  const historial = ref([]) 
  
  const formularioEdicion = reactive({
    titulo: '',
    descripcion: '',
    id_RamaJuridica: null,
    id_estatus: null,
    id: null,
    id_usuario: null 
  })
  
  const formatearFechaHistorial = (fechaIso) => {
    const fecha = new Date(fechaIso)
    return fecha.toLocaleDateString('es-MX', { day: '2-digit', month: 'short', year: '2-digit' }).toUpperCase()
  }
  
  const cargarDetalleCaso = async () => {
    const token = localStorage.getItem('token')
    if (!token) { router.push('/login'); return }
    const idCaso = route.params.id
    try {
      const respuesta = await axios.get(`http://localhost:5039/api/casos/${idCaso}`, {
        headers: { Authorization: `Bearer ${token}` }
      })
      caso.value = respuesta.data
    } catch (error) {
      console.error("Error al cargar el caso", error)
    } finally {
      cargando.value = false
    }
  }
  
  const cargarHistorial = async () => {
    const token = localStorage.getItem('token')
    if (!token) return
    try {
      const respuesta = await axios.get(`http://localhost:5039/api/casos/${route.params.id}/historial`, {
        headers: { Authorization: `Bearer ${token}` }
      })
      historial.value = respuesta.data
    } catch (error) {
      console.error("Error al cargar el historial", error)
    }
  }
  
  const cargarCatalogos = async () => {
    const token = localStorage.getItem('token')
    try {
      const [resRamas, resEstatus] = await Promise.all([
        axios.get('http://localhost:5039/api/RamaJuridica', { headers: { Authorization: `Bearer ${token}` } }),
        axios.get('http://localhost:5039/api/Estatus', { headers: { Authorization: `Bearer ${token}` } })
      ])
      listaRamas.value = resRamas.data
      listaEstatus.value = resEstatus.data
    } catch (error) {
      console.error("Error cargando catálogos", error)
    }
  }
  
  const activarEdicion = () => {
    if (!caso.value) return
    formularioEdicion.id = caso.value.id
    formularioEdicion.titulo = caso.value.titulo
    formularioEdicion.descripcion = caso.value.descripcion
    formularioEdicion.id_RamaJuridica = caso.value.id_RamaJuridica
    formularioEdicion.id_estatus = caso.value.id_estatus
    formularioEdicion.id_usuario = caso.value.id_usuario 
    editando.value = true 
  }
  
  const cancelarEdicion = () => {
    editando.value = false
  }
  
  const guardarCambios = async () => {
    if (!caso.value) return
    const token = localStorage.getItem('token')
    guardando.value = true
    
    try {
      await axios.put(`http://localhost:5039/api/casos/${formularioEdicion.id}`, formularioEdicion, {
        headers: { Authorization: `Bearer ${token}` }
      })
      
      await cargarDetalleCaso()
      await cargarHistorial() 
      
      editando.value = false
    } catch (error) {
      console.error(error)
      alert("Error al actualizar. Verifica los datos.")
    } finally {
      guardando.value = false
    }
  }
  
  const eliminarCaso = async () => {
    const confirmacion = window.confirm("⚠️ ¿ESTÁS SEGURO?\n\nEsta acción borrará el expediente y todo su historial de forma permanente. Esta acción no se puede deshacer.");
    
    if (!confirmacion) return;
  
    const token = localStorage.getItem('token');
    try {
      await axios.delete(`http://localhost:5039/api/casos/${route.params.id}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      
      alert("Expediente eliminado correctamente.");
      router.push('/casos');
    } catch (error) {
      console.error(error);
      alert("Error al eliminar. Revisa la conexión con el servidor.");
    }
  }
  
  onMounted(() => {
    cargarDetalleCaso()
    cargarCatalogos()
    cargarHistorial() 
  })
  </script>