import Image from 'next/image'
import equipos from '../public/images/equipos.jpg'
import momentos from '../public/images/momentos.jpg'


export default function Home() {
  return (
    <>
      <div className="text-center">
        <h1 className="text-c-primary">Bienvenido a la mejor cobertura</h1>
        <p className="lead">Con planes confiables, 5G ultrarrápido y un soporte premiado, tienes todo para estar siempre conectado.</p>
      </div>
      <hr />
      <div className="row">
        <div className="col-md-5" >
          <Image
            src={equipos}
            width={640}
            height={553}
            layout="responsive"
            alt="Equipos" />
        </div>
        <div className="col-md-7" >
          <h1 className="mb-4 text-c-tint-90">Los mejores equipos</h1>
          <p> Sabemos que tener el mejor equipo <strong className='text-c-primary'>marca la diferencia</strong>. Por eso, te ofrecemos los smartphones más avanzados del mercado,
            con tecnología de última generación, cámaras de alta resolución y baterías que te acompañan todo el día.
          </p>
          <p>Ya sea que busques productividad, entretenimiento o conectividad total, tenemos el dispositivo ideal para ti.

            Además, contamos con promociones exclusivas y facilidades de pago para que estrenes sin preocuparte. Con los mejores equipos y la mejor red, estar conectado nunca había sido tan fácil y emocionante. ¡Descubre hoy mismo el equipo que va contigo y lleva tu experiencia móvil al siguiente nivel!</p>
        </div>
      </div>
      <hr />
      <div className="row">
        <div className="col-md-7" >
          <h1 className="mb-4">Los mejores momentos</h1>
          <p>Sabemos lo importante que es estar siempre conectado, <strong className='text-c-primary'>sin importar a dónde vayas.</strong> </p>
          <p>Por eso, nuestra red ofrece una cobertura increíble que te acompaña incluso en las zonas de difícil acceso, como en carretera o en medio de la naturaleza.
            Nuestra red está diseñada para ofrecerte una experiencia de conexión fluida y confiable, ya sea que estés en casa, en la oficina o explorando el aire libre.</p>
          <p>
            Ya sea que estés de roadtrip, explorando nuevos destinos o trabajando en movimiento, puedes confiar en que nuestra señal te mantendrá cerca de lo que más importa.</p>
        </div>
        <div className="col-md-5" >
          <Image
            src={momentos}
            width={640}
            height={553}
            layout="responsive"
            alt="Momentos" />
        </div>
      </div>
    </>

  )
}
