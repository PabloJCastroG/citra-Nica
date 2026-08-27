import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  Image,
} from 'react-native';
import {
  Menu,
  Bell,
  Megaphone,
  Calendar,
  FileText,
  List,
  BadgeCheck,
} from 'lucide-react-native';

export default function InstitutionHomeScreen() {
  return (
    <ScrollView style={styles.container} showsVerticalScrollIndicator={false}>
      {/* 1. Header */}
      <View style={styles.header}>
        <TouchableOpacity>
          <Menu color="#000" size={28} />
        </TouchableOpacity>
        <Image
          source={require('../../assets/images/logo.png')}
          style={styles.logo}
          resizeMode="contain"
        />
        <TouchableOpacity>
          <Bell color="#000" size={24} />
        </TouchableOpacity>
      </View>

      {/* 2. Tarjeta de Bienvenida */}
      <View style={styles.welcomeCard}>
        <View style={styles.welcomeTextContainer}>
          <Text style={styles.welcomeTitle}>
            ¡Hola, INTA, Instituto Nacional de Tecnología Agropecuaria!
          </Text>
          <Text style={styles.welcomeSubtitle}>
            Ayudando al desarrollo del campo Nicaragüense
          </Text>
        </View>
        <View style={styles.welcomeLogoPlaceholder}>
          <Text style={styles.welcomeLogoText}>INTA</Text>
        </View>
      </View>

      {/* 3. Resumen de Actividad */}
      <Text style={styles.sectionTitle}>Resumen de actividad</Text>
      <View style={styles.statsGrid}>
        <View style={styles.statCard}>
          <Megaphone color="#15803d" size={32} />
          <Text style={styles.statLabel}>Comunicados</Text>
          <Text style={styles.statValue}>12</Text>
        </View>
        <View style={styles.statCard}>
          <Calendar color="#ea580c" size={32} />
          <Text style={styles.statLabel}>Eventos</Text>
          <Text style={styles.statValue}>8</Text>
        </View>
        <View style={styles.statCard}>
          <FileText color="#15803d" size={32} />
          <Text style={styles.statLabel}>Documentos</Text>
          <Text style={styles.statValue}>15</Text>
        </View>
      </View>

      <View style={styles.statsGridBottom}>
        <View style={[styles.statCardHorizontal, { flex: 1, marginRight: 10 }]}>
          <List color="#7c3aed" size={28} />
          <View style={styles.statTextCol}>
            <Text style={styles.statLabel}>Formularios</Text>
            <Text style={styles.statValue}>6</Text>
          </View>
        </View>
        <View style={[styles.statCardHorizontal, { flex: 1 }]}>
          <BadgeCheck color="#0284c7" size={28} />
          <View style={styles.statTextCol}>
            <Text style={styles.statLabel}>Productores verificados</Text>
            <Text style={styles.statValue}>48</Text>
          </View>
        </View>
      </View>

      {/* 4. Acciones Rápidas */}
      <Text style={styles.sectionTitle}>Acciones Rápidas</Text>
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        style={styles.quickActionsScroll}
      >
        {['Comunicado', 'Evento', 'Documento', 'Formulario'].map(
          (action, index) => (
            <TouchableOpacity key={index} style={styles.actionButton}>
              {index === 0 && <Megaphone color="#15803d" size={24} />}
              {index === 1 && <Calendar color="#ea580c" size={24} />}
              {index === 2 && <FileText color="#15803d" size={24} />}
              {index === 3 && <List color="#7c3aed" size={24} />}
              <Text style={styles.actionLabel}>{action}</Text>
            </TouchableOpacity>
          ),
        )}
      </ScrollView>

      {/* 5. Publicaciones Recientes */}
      <Text style={styles.sectionTitle}>Publicaciones Recientes</Text>
      <View style={styles.listItem}>
        <View style={[styles.listIconBg, { backgroundColor: '#dcfce7' }]}>
          <Megaphone color="#15803d" size={20} />
        </View>
        <View style={styles.listTextContainer}>
          <Text style={styles.listTitle}>
            Nuevo programa de apoyo a productores
          </Text>
          <Text style={styles.listSubtitle}>publicado hace 2 horas</Text>
        </View>
      </View>

      <View style={styles.listItem}>
        <View style={[styles.listIconBg, { backgroundColor: '#ffedd5' }]}>
          <Calendar color="#ea580c" size={20} />
        </View>
        <View style={styles.listTextContainer}>
          <Text style={styles.listTitle}>
            Feria agrícola municipal Managua 2026
          </Text>
          <Text style={styles.listSubtitle}>25 de mayo, 2026</Text>
        </View>
      </View>

      {/* 6. Próximos Eventos */}
      <Text style={styles.sectionTitle}>Próximos eventos</Text>
      <View style={styles.listItem}>
        <View style={[styles.listIconBg, { backgroundColor: '#ffedd5' }]}>
          <Calendar color="#ea580c" size={20} />
        </View>
        <View style={styles.listTextContainer}>
          <Text style={styles.listTitle}>Taller de poda de cítricos</Text>
          <View style={styles.eventDetails}>
            <Text style={styles.listSubtitle}>20 de junio, 2026 9:00 a.m</Text>
            <Text style={styles.listSubtitle}>
              Jinotega, San Rafael del Norte
            </Text>
          </View>
        </View>
      </View>

      {/* Espaciado final para que no quede oculto detrás del Bottom Tab Navigation */}
      <View style={{ height: 40 }} />
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    paddingHorizontal: 16,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 50,
    marginBottom: 20,
  },
  logo: {
    height: 40,
    width: 120,
  },
  welcomeCard: {
    backgroundColor: '#2b7a4b', // Verde característico
    borderRadius: 12,
    padding: 16,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: 24,
  },
  welcomeTextContainer: {
    flex: 1,
    paddingRight: 10,
  },
  welcomeTitle: {
    color: '#fff',
    fontSize: 14,
    fontWeight: 'bold',
    marginBottom: 4,
  },
  welcomeSubtitle: {
    color: '#e2e8f0',
    fontSize: 12,
  },
  welcomeLogoPlaceholder: {
    width: 60,
    height: 60,
    borderRadius: 30,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
  welcomeLogoText: {
    color: '#2b7a4b',
    fontWeight: 'bold',
    fontSize: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#1f2937',
    marginBottom: 12,
  },
  statsGrid: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 12,
  },
  statCard: {
    backgroundColor: '#fff',
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
    width: '31%',
    borderWidth: 1,
    borderColor: '#f3f4f6',
    elevation: 2, // Sombra para Android
    shadowColor: '#000', // Sombra para iOS
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.1,
    shadowRadius: 2,
  },
  statsGridBottom: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 24,
  },
  statCardHorizontal: {
    flexDirection: 'row',
    backgroundColor: '#fff',
    borderRadius: 12,
    padding: 12,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#f3f4f6',
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.1,
    shadowRadius: 2,
  },
  statTextCol: {
    marginLeft: 10,
  },
  statLabel: {
    fontSize: 11,
    color: '#6b7280',
    marginTop: 4,
  },
  statValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#111827',
  },
  quickActionsScroll: {
    marginBottom: 24,
  },
  actionButton: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 16,
    alignItems: 'center',
    marginRight: 12,
    width: 80,
    borderWidth: 1,
    borderColor: '#f3f4f6',
  },
  actionLabel: {
    fontSize: 10,
    color: '#6b7280',
    marginTop: 8,
    textAlign: 'center',
  },
  listItem: {
    flexDirection: 'row',
    backgroundColor: '#fff',
    borderRadius: 12,
    padding: 12,
    marginBottom: 12,
    borderWidth: 1,
    borderColor: '#f3f4f6',
    alignItems: 'center',
  },
  listIconBg: {
    width: 40,
    height: 40,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 12,
  },
  listTextContainer: {
    flex: 1,
  },
  listTitle: {
    fontSize: 13,
    fontWeight: 'bold',
    color: '#111827',
  },
  listSubtitle: {
    fontSize: 11,
    color: '#6b7280',
    marginTop: 2,
  },
  eventDetails: {
    marginTop: 4,
  },
});
