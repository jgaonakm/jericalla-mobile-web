import { GetServerSideProps } from 'next'

type Plan = {
  id: string
  name: string
  price: string
  features: string[]
}

type Props = {
  plans: Plan[]
}

export default function Plans({ plans }: Props) {
  return (
    <>
      <h1 className="mb-4">Our Mobile Plans</h1>
      <div className="row">
        {plans.map(plan => (
          <div className="col-md-6" key={plan.id}>
            <div className="card mb-4">
              <div className="card-body">
                <h5 className="card-title">{plan.name}</h5>
                <h6 className="card-subtitle mb-2 text-muted">{plan.price} / month</h6>
                <ul className="list-group list-group-flush">
                  {plan.features.map((f, i) => (
                    <li className="list-group-item" key={i}>{f}</li>
                  ))}
                </ul>
                <a href="#" className="btn btn-primary mt-3">Choose Plan</a>
              </div>
            </div>
          </div>
        ))}
      </div>
    </>
  )
}

export const getServerSideProps: GetServerSideProps = async () => {
  const plans: Plan[] = [
    { id: '1', name: 'Basic', price: '$20', features: ['5GB Data', 'Unlimited Calls'] },
    { id: '2', name: 'Premium', price: '$45', features: ['Unlimited Data', '5G Access', 'Roaming'] },
  ]

  return {
    props: { plans }
  }
}
