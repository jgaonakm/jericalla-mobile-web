import Image from 'next/image'
import camping from '../public/images/camping.jpg'

export default function Coverage() {
  return (
    <section>
      <div className="row">
        <div className="col-md-5" >
          <Image
            src={camping}
            width={640}
            height={553}
            layout="responsive"
            alt="Cobertura" />
        </div>
        <div className="col-md-7" >
          <h1 className="mb-4">Nuestra cobertura</h1>
          <p>Sabemos lo importante que es estar siempre conectado, <strong>sin importar a dónde vayas.</strong> </p>
          <p>Por eso, nuestra red ofrece una cobertura increíble que te acompaña incluso en las zonas de difícil acceso, como en carretera o en medio de la naturaleza.
            Nuestra red está diseñada para ofrecerte una experiencia de conexión fluida y confiable, ya sea que estés en casa, en la oficina o explorando el aire libre.</p>
          <p>
            Ya sea que estés de roadtrip, explorando nuevos destinos o trabajando en movimiento, puedes confiar en que nuestra señal te mantendrá cerca de lo que más importa.</p>
        </div>
      </div>
    </section>
  )
}
